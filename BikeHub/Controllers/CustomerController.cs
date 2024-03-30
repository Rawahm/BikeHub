using BikeHub.Models;
using Microsoft.AspNetCore.Mvc;
using BikeHub.Data;
using Microsoft.EntityFrameworkCore;


namespace BikeHub.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BikeHubDBContext dbContext;
        public CustomerController(BikeHubDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    StudentId = viewModel.StudentId,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    PhoneNumber = viewModel.PhoneNumber,
                    Email = viewModel.Email,
                    CampusName = viewModel.CampusName,
                    EmergencyContactName = viewModel.EmergencyContactName,
                    EmergencyContactNum = viewModel.EmergencyContactNum,
                    TypeOfCustomer = viewModel.TypeOfCustomer,
                    TypeOfRider = viewModel.TypeOfRider,
                    TAndCAgreement = viewModel.TAndCAgreement,
                    EmailSubscription = viewModel.EmailSubscription
                };

                await dbContext.CustomerInformation.AddAsync(customer);
                await dbContext.SaveChangesAsync();

                // Redirect to a success page or take appropriate action
                return RedirectToAction("RegisterSuccess");
                //Create a view for RegisterSuccess here Include Get request
            }
            // If model state is not valid, return the view with validation errors
            return View(viewModel);
        }
        */
        /* For Testing purposes only 
         * This method has no if statement to validate the submitted info 
         */ 
        public async Task<IActionResult> Register(RegisterCustomerViewModel viewModel)
        {
            var customer = new Customer
            {
                StudentId = viewModel.StudentId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email,
                CampusName = viewModel.CampusName,
                EmergencyContactName = viewModel.EmergencyContactName,
                EmergencyContactNum = viewModel.EmergencyContactNum,
                TypeOfCustomer = viewModel.TypeOfCustomer,
                TypeOfRider = viewModel.TypeOfRider,
                TAndCAgreement = viewModel.TAndCAgreement,
                EmailSubscription = viewModel.EmailSubscription
            };

            await dbContext.CustomerInformation.AddAsync(customer);
            await dbContext.SaveChangesAsync();

            // Redirect to a success page or take appropriate action
            return RedirectToAction("RegisterSuccess");
            
        }
        
        [HttpGet]
        public async Task<IActionResult> AdminList()
        {
            var customers = await dbContext.CustomerInformation.ToListAsync();
            return View(customers);


        }
        // GET: /Customer/RegisterSuccess
        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }
    }
}

