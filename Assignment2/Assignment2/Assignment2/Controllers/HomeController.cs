using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Error handling actions
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult StatusCode(int code)
        {
            if (code == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View("Error");
            }
        }
    }
}