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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult ErrorForbidden()
        {
            return View("ErrorForbidden");
        }

        [Route("Error/{statusCode}")]
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 403)
            {
                return View("ErrorForbidden");
            }
            if (statusCode == 404)
            {
                return View("404");
            }
            return View("OtherError");
        }
    }
}