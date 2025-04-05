using System.Security.Cryptography;
using COMP2139_Labs.Data;
using COMP2139_Labs.Models;
using COMP2139_Labs.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


namespace COMP2139_Labs.Areas.ProjectManagement.Controllers;
[Area("ProjectManagement")]//access the area first then specify the route
[Route("[area]/[controller]/[action]")]
public class ProjectController : Controller
{
    private readonly ILogger<ProjectController> _logger;
    private readonly ApplicationDbContext _context;
    public ProjectController(ApplicationDbContext context, ILogger<ProjectController> logger)
    {
        _context = context;
        _logger = logger;
        
    }
    [HttpGet("")] //default - use get request especially for index action(default action without the name)
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Accessed ProjectController Index at {Time}", DateTime.Now);
        //retrieve all the projects from database
        
        var projects = await _context.Projects.ToListAsync();
        return View(projects);
    }

    [HttpGet("Create")]//optional to provide:default
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create( Project project )//from the url body not the url
    {
        // Convert dates to UTC before saving
        project.StartDate = DateTime.SpecifyKind(project.StartDate.Date, DateTimeKind.Utc);
        project.EndDate = DateTime.SpecifyKind(project.EndDate.Date, DateTimeKind.Utc);
        //Database -->persist the new project to the database
        if (ModelState.IsValid)
        {
            _context.Projects.Add(project); //Add project to database
            await _context.SaveChangesAsync();         //Saves changed to database
            return RedirectToAction("Index");
        }
        return View(project);
    }
    //CRUD - create, read , update, delete
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)//id from the url
    {
        _logger.LogInformation("Accessed Project Details at {Time}", DateTime.Now);
        //Database --> Retrieve project from database if it exists otherwise return null
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            _logger.LogWarning("Could not find Project with id of {id}", id);
            return NotFound();//404
        }
        return View(project);
    }
    
    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        //Database --> Retrieve project from database if it exists otherwise return null
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();//404
        }
        return View(project);//it is going to show the values in the places
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name, Description")] Project project)//id comes from the url but the project come from hte body of the request
    {
        //[Bind] ensures only the specified properties are updated.
        if (id != project.ProjectId)
        {
            return NotFound();//ensures the id in the route matches the ID Model
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Projects.Update(project); //updates the project
                await _context.SaveChangesAsync(); //commit
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!await ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                
            }
            return RedirectToAction("Index");
        }
        return View(project);
        
    }

    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }
    
    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        //Database --> RDelete project from database if it exists otherwise return null
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId==id);
        if (project == null)
        {
            return NotFound();//404
        }
        return View(project);//it is going to show the values in the places
    }

    [HttpPost("Delete/{ProjectId:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int ProjectId)
    {
        var project = await _context.Projects.FindAsync(ProjectId);
        if (project != null)
        {
            _context.Projects.Remove(project);//remove project from database
            await _context.SaveChangesAsync(); //commit the changes to database
            return RedirectToAction("Index");
        }
        return NotFound();
    }

    [HttpGet("Search/{searchString}")]
    public async Task<IActionResult> Search(string searchString)
    {
        var projectsQuery = _context.Projects.AsQueryable();
        bool searchPerformed= !string.IsNullOrWhiteSpace(searchString);
        if (searchPerformed)
        {
            searchString=searchString.ToLower();
            projectsQuery=projectsQuery.Where(p => p.Name.ToLower().Contains(searchString)
                                                || p.Description.ToLower().Contains(searchString));
            
        }
        //Asynchronous execution means this method does not block the thread while waiting for the database
        var projects = await projectsQuery.ToListAsync();
        //pass search metadata 
        ViewData["SearchString"] = searchString;//temporary to pass info to the resultant view. but it is gone after the request
        ViewData["SearchPerformed"] = searchPerformed;
        
        return View("Index", projects);
    }
    
    


}