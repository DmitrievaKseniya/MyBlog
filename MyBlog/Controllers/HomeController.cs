using Microsoft.AspNetCore.Mvc;
using MyBlog.WebService.Models;
using System.Diagnostics;

namespace MyBlog.WebService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ErrorForbidden()
        {
            return View("ErrorForbidden");
        }

        public IActionResult Error()
        {
            return View("OtherError");
        }

        [Route("Error/{statusCode}")]
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                403 => View("ErrorForbidden"),
                404 => View("404"),
                _ => View("OtherError")
            };
        }
    }
}