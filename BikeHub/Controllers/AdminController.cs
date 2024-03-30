using BikeHub.Data;
using BikeHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
/*
 * @Author : Rawah Almesri 
 * 2024-03-18
 */

namespace BikeHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly BikeHubDBContext dbContext;

        public AdminController(BikeHubDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET action for the search form
        [HttpGet]
        public IActionResult SearchCustomer()
        {
            return View();
        }
        // POST action to handle form submission and retrieve customer record
        [HttpPost]
        public async Task<IActionResult> SearchCustomer(int customerId)
        {
            var customer = await dbContext.CustomerInformation.FindAsync(customerId);

            if (customer == null)
            {
                // Handle case where customer with given ID is not found
                TempData["Message"] = "Customer with ID " + customerId + " was not found";
                return RedirectToAction("AdminList");
            }

            return View("CustomerInfo", customer);
        }
        // Navigate to the EditCustomer page 
        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var customer = await dbContext.CustomerInformation.FindAsync(id);
            return View(customer);
        }
        // post request to upadte the record
        [HttpPost]
        public async Task<IActionResult> EditCustomer(Customer viewModel)
        {
            var customer = await dbContext.CustomerInformation.FindAsync(viewModel.StudentId);
            if (customer is not null)
            {
                customer.StudentId = viewModel.StudentId;
                customer.FirstName = viewModel.FirstName;
                customer.LastName = viewModel.LastName;
                customer.Email = viewModel.Email;
                customer.CampusName = viewModel.CampusName;
                customer.PhoneNumber = viewModel.PhoneNumber;
                customer.TypeOfCustomer = viewModel.TypeOfCustomer;
                customer.TypeOfRider = viewModel.TypeOfRider;
                customer.EmergencyContactName = viewModel.EmergencyContactName;
                customer.EmergencyContactNum = viewModel.EmergencyContactNum;
                customer.EmailSubscription = viewModel.EmailSubscription;
                customer.TAndCAgreement = viewModel.TAndCAgreement;
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("UpdateSuccess");




        }


        public IActionResult Index()
        {
            return View();
        }
        // Redirect to Success Msg when update new record
        [HttpGet]
        public IActionResult UpdateSuccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteSuccess()
        {
            return View();
        }

        // We need post request:for the job then Get request: conformation msg of deletion
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await dbContext.CustomerInformation.FirstOrDefaultAsync(x => x.StudentId == id);
            if (customer != null)
            {
                dbContext.CustomerInformation.Remove(customer);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("DeleteSuccess");
            }
            return RedirectToAction("Index"); // Redirect to the home page if customer not found
        }
        [HttpGet]
        public async Task<IActionResult> AdminList()
        {
            var customers = await dbContext.CustomerInformation.ToListAsync();
            return View(customers);


        }
    }
}
