using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeaOfShops.Domain.Entities;
using SeaOfShops.Filters;
using SeaOfShops.Models;
using System.Diagnostics;

namespace SeaOfShops.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        [DateTimeExecutionFilter]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public IActionResult Index()
        {
            ViewBag.CountUsers = _userManager.Users.Count();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}