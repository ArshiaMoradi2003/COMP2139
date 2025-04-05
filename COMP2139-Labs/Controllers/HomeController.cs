using System.Diagnostics;
using COMP2139_Labs.Areas.ProjectManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using COMP2139_Labs.Models;

namespace COMP2139_Labs.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        //if it is lower it will be logged
        _logger.LogInformation("Accessed HomeController Index at {Time}", DateTime.Now);
        return View();
    }

    public IActionResult About()
    {
        _logger.LogInformation("Accessed HomeController About at {Time}", DateTime.Now);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("Accessed HomeController Index at {Time}", DateTime.Now);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        _logger.LogInformation("Accessed HomeController GeneralSearch at {Time}", DateTime.Now);
        searchType = searchType.Trim().ToLower();
        if (string.IsNullOrWhiteSpace(searchString) || string.IsNullOrWhiteSpace(searchType))
        {
            return RedirectToAction(nameof(Index),"Home");
        }
        if (searchType == "projects")
        {
            return RedirectToAction(nameof(ProjectController.Search), "Project", new { area="ProjectManagement", searchString });
        }

        else if (searchType == "tasks") 
        {
            return RedirectToAction(nameof(ProjectTaskController.Search), "ProjectTask", new { area="ProjectManagement", searchString });
        }
        return RedirectToAction(nameof(Index),"Home");
        
    }

    public IActionResult NotFound(int statusCode)
    {
        _logger.LogWarning("Not found invoked at {Time}", DateTime.Now);
        if (statusCode == 404)
        {
            return ViewBag("NotFound");
        }
        return View("Error");
    }
}