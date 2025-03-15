using Microsoft.AspNetCore.Mvc;

namespace COMP2139_Friday.Controllers;

public class AboutController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}