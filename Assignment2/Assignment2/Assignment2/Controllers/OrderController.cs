using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;
using Assignment2.Services;
using System.Threading.Tasks;

namespace Assignment2.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Create Order Action
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrderAsync(order);
                return RedirectToAction(nameof(Confirmation));
            }
            return View(order);
        }

        // Order Confirmation Action
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}