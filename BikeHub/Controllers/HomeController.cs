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
            // IEnumerable<Customer> CustomerInformation = _db.CustomerInformation.ToList();

            //return View(CustomerInformation);
            var pageContents = _db.PageContent.ToList(); // Retrieve content from the database
            return View(pageContents); // Pass the content to the view
        }

        public IActionResult About()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs(string name, string email, string message)
        {
            // Check if contact us form is valid 
            if (ModelState.IsValid)
            {
                // Create a new ContactUs object
                var contactUs = new ContactUs
                {
                    Name = name,
                    Email = email,
                    Message = message
                };

                // Add the object to the DbSet and save it to the database
                _db.ContactUsMessages.Add(contactUs);
                await _db.SaveChangesAsync();

                //  display a confirmation message 
                TempData["Message"] = "Thank you for reaching out! We will get back to you soon.";
                return RedirectToAction("ContactUs");
            }

            return View("ContactUs");
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult ViewContactMessages()
        {
            var contactMessages = _db.ContactUsMessages.ToList();
            return View(contactMessages);
        }
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

    }
}
