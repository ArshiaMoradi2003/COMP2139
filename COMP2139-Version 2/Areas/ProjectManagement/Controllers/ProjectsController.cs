using COMP2139_Friday.Data;
using COMP2139_Friday.Models;
using COMP2139_Friday.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace COMP2139_Friday.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("projects")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context; // Save context to a private field
        }

        // GET: /projects
        [HttpGet("")]
        public IActionResult Index()
        {
            var projects = _context.Projects.ToList(); // Retrieve all projects from the database
            ViewBag.SearchPerformed = false; // No search performed
            return View(projects);
        }

        // GET: /projects/search?searchTerm=xyz
        [HttpGet("search")]
        public IActionResult Search(string searchTerm)
        {
            bool searchPerformed = !string.IsNullOrWhiteSpace(searchTerm);

            var projects = searchPerformed
                ? _context.Projects
                    .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) || 
                                p.Description.ToLower().Contains(searchTerm.ToLower()))
                    .ToList()
                : _context.Projects.ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.SearchPerformed = searchPerformed;
            return View("Index", projects);
        }

        // GET: /projects/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /projects/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,StartDate,EndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Remove seconds from StartDate and EndDate
                    project.StartDate = new DateTime(project.StartDate.Year, project.StartDate.Month, project.StartDate.Day, project.StartDate.Hour, project.StartDate.Minute, 0, DateTimeKind.Utc);
                    project.EndDate = new DateTime(project.EndDate.Year, project.EndDate.Month, project.EndDate.Day, project.EndDate.Hour, project.EndDate.Minute, 0, DateTimeKind.Utc);

                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating project: " + ex.Message);
                }
            }

            return View(project);
        }


        // GET: /projects/{id}
        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id); // Retrieve project by ID
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: /projects/edit/{id}
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id); // Retrieve project by ID
            if (project == null)
            {
                return NotFound();
            }

            return View(project); // Pass project to the Edit view
        }

        // POST: /projects/edit/{id}
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Description,StartDate,EndDate")] Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest(); // Ensure the ID in the route matches the project ID
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Remove seconds from StartDate and EndDate
                    project.StartDate = new DateTime(project.StartDate.Year, project.StartDate.Month, project.StartDate.Day, project.StartDate.Hour, project.StartDate.Minute, 0, DateTimeKind.Utc);
                    project.EndDate = new DateTime(project.EndDate.Year, project.EndDate.Month, project.EndDate.Day, project.EndDate.Hour, project.EndDate.Minute, 0, DateTimeKind.Utc);

                    _context.Projects.Update(project); // Update the project in the database
                    await _context.SaveChangesAsync(); // Save changes
                    return RedirectToAction("Index"); // Redirect to the project list
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating project: " + ex.Message);
                }
            }

            return View(project); // Return to Edit view if validation fails
        }


        // GET: /projects/delete/{id}
        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id); // Retrieve project by ID
            if (project == null)
            {
                return NotFound();
            }

            return View(project); // Pass project to the Delete view
        }

        // POST: /projects/delete/{id}
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
