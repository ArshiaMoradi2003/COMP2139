using System.Diagnostics; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COMP2139_Friday.Models;
using COMP2139_Friday.Data; // Make sure this namespace is included

namespace COMP2139_Friday.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    // Update the constructor to accept ApplicationDbContext
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // New Search action to handle the generic search requests from the navbar
    public async Task<IActionResult> Search(string searchType, string searchTerm)
    {
        ViewBag.SearchTerm = searchTerm;  // Pass the search term to the view

        if (searchType == "projects")
        {
            var projects = await _context.Projects
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();
            return View("ProjectsSearch", projects);
        }
        else if (searchType == "tasks")
        {
            var tasks = await _context.ProjectTasks
                .Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))
                .ToListAsync();
            return View("TasksSearch", tasks);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
