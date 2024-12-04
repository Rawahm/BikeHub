using BikeHub.Models;
using Microsoft.AspNetCore.Mvc;
using BikeHub.Data;
using Microsoft.EntityFrameworkCore;

/*
 * @Author : Rawah Almesri 
 * 2024-03-11
 */


namespace BikeHub.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BikeHubDBContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CustomerController(BikeHubDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Handle Waiver agreement
                if (!viewModel.TAndCAgreement)
                {
                    ModelState.AddModelError("TAndCAgreement", "You must agree to the terms and conditions.");
                    return View(viewModel); // Return the view with the error
                }

                // Handle File Upload if EnrollmentLetter is provided
                if (viewModel.EnrollmentLetter != null)
                {
                    var fileExtension = Path.GetExtension(viewModel.EnrollmentLetter.FileName);
                    if (fileExtension.ToLower() != ".pdf")
                    {
                        ModelState.AddModelError("EnrollmentLetter", "Only .pdf files are allowed.");
                        return View(viewModel);
                    }

                    // Save the uploaded file to a folder
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Create directory if not exists
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewModel.EnrollmentLetter.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.EnrollmentLetter.CopyToAsync(fileStream);
                    }
                }

                // Insert customer data into the database
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

                // Redirect to a success page
                return RedirectToAction("RegisterSuccess");
            }

            // If model state is not valid, return the view with validation errors
            return View(viewModel);
        }


        /* For Testing purposes only 
         * This method has no if statement to validate the submitted info 
         *
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

            // Redirect to a success page    /Customer/RegisterSuccess

            return RedirectToAction("RegisterSuccess");
            
        }
        */
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

