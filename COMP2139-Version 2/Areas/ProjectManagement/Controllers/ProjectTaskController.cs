using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COMP2139_Friday.Data;
using COMP2139_Friday.Models;
using COMP2139_Friday.Areas.ProjectManagement.Models;
using System;

namespace COMP2139_Friday.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("projects/{projectId:int}/tasks")]
    public class ProjectTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /projects/{projectId}/tasks
        [HttpGet("")]
        public async Task<IActionResult> Index(int projectId)
        {
            var projectTasks = await _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
            ViewBag.ProjectId = projectId;
            ViewBag.SearchPerformed = false;
            return View(projectTasks);
        }

        // GET: /projects/{projectId}/tasks/details/{id}
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int projectId, int id)
        {
            var projectTask = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (projectTask == null)
                return NotFound();

            ViewBag.ProjectId = projectId;
            return View(projectTask);
        }

        // GET: /projects/{projectId}/tasks/create
        [HttpGet("create")]
        public IActionResult Create(int projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        // POST: /projects/{projectId}/tasks/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int projectId, ProjectTask projectTask)
        {
            projectTask.ProjectId = projectId;

            // Convert StartDate and EndDate to UTC to avoid PostgreSQL error
            projectTask.StartDate = DateTime.SpecifyKind(projectTask.StartDate, DateTimeKind.Utc);
            projectTask.EndDate = DateTime.SpecifyKind(projectTask.EndDate, DateTimeKind.Utc);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Validation Errors:");
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                ViewBag.ProjectId = projectId;
                return View(projectTask);
            }

            try
            {
                _context.Add(projectTask);
                await _context.SaveChangesAsync();
                Console.WriteLine($" Successfully saved Task: {projectTask.Title} for Project ID: {projectTask.ProjectId}");
                return RedirectToAction("Index", new { projectId = projectTask.ProjectId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error while saving task: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the task.");
                ViewBag.ProjectId = projectId;
                return View(projectTask);
            }
        }

        // GET: /projects/{projectId}/tasks/edit/{id}
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int projectId, int id)
        {
            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask == null)
                return NotFound();

            ViewBag.ProjectId = projectId;
            return View(projectTask);
        }

        // POST: /projects/{projectId}/tasks/edit/{id}
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int projectId, int id, ProjectTask projectTask)
        {
            if (id != projectTask.ProjectTaskId)
                return NotFound();

            try
            {
                // Ensure DateTime fields are explicitly set to UTC
                projectTask.StartDate = DateTime.SpecifyKind(projectTask.StartDate, DateTimeKind.Utc);
                projectTask.EndDate = DateTime.SpecifyKind(projectTask.EndDate, DateTimeKind.Utc);

                _context.Update(projectTask);
                await _context.SaveChangesAsync();
                Console.WriteLine($" Successfully updated Task: {projectTask.Title}");
                return RedirectToAction("Index", new { projectId = projectTask.ProjectId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error while updating task: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the task.");
                ViewBag.ProjectId = projectId;
                return View(projectTask);
            }
        }

        // GET: /projects/{projectId}/tasks/delete/{id}
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int projectId, int id)
        {
            var projectTask = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (projectTask == null)
                return NotFound();

            ViewBag.ProjectId = projectId;
            return View(projectTask);
        }

        // POST: /projects/{projectId}/tasks/delete/{id}
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int projectId, int id)
        {
            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask != null)
            {
                try
                {
                    _context.ProjectTasks.Remove(projectTask);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($" Successfully deleted Task ID: {id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error while deleting task: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while deleting the task.");
                    return View(projectTask);
                }
            }
            return RedirectToAction("Index", new { projectId });
        }
    }
}
