using BikeHub.Data;
using BikeHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BikeHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BikeHubDBContext _db;

        public HomeController(ILogger<HomeController> logger, BikeHubDBContext db)
        {
            _db = db;

            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Customer> CustomerInformation = _db.CustomerInformation.ToList();

            return View(CustomerInformation);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
