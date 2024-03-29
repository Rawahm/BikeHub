using Microsoft.AspNetCore.Mvc;

namespace BikeHub.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
