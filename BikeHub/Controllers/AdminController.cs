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
        /* This is search customer by ID only it replaced with the other method that search by other terms
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
        */
        public async Task<IActionResult> SearchCustomer(string searchType, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                TempData["Message"] = "Please enter a search term.";
                return RedirectToAction("AdminList");
            }

            Customer? customer = null;
        
            switch (searchType)
            {
                case "StudentId":
                    if (int.TryParse(searchTerm, out int id))
                    {
                        customer = await dbContext.CustomerInformation.FirstOrDefaultAsync(c => c.StudentId == id);
                    }
                    break;

                case "Name":
                    customer = await dbContext.CustomerInformation
                        .FirstOrDefaultAsync(c => c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm));
                    break;

                case "Email":
                    customer = await dbContext.CustomerInformation
                        .FirstOrDefaultAsync(c => c.Email.Contains(searchTerm));
                    break;

                default:
                    TempData["Message"] = "Invalid search type selected.";
                    return RedirectToAction("AdminList");
            }

            if (customer == null)
            {
                TempData["Message"] = "No customer found matching the search term: " + searchTerm;
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
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var customer = await dbContext.CustomerInformation.FirstOrDefaultAsync(x => x.StudentId == id);
                    if (customer != null)
                    {
                        // Archive the customer record
                        await ArchiveCustomer(customer);

                        // Remove the customer record from the main table
                        dbContext.CustomerInformation.Remove(customer);
                        await dbContext.SaveChangesAsync();

                        // Commit the transaction
                        transaction.Commit();

                        return RedirectToAction("DeleteSuccess");
                    }
                    return RedirectToAction("Index"); // Redirect to the home page if customer not found
                }
                catch (Exception)
                {
                    // Rollback the transaction in case of an exception
                    transaction.Rollback();
                    throw; // Re-throw the exception to be handled elsewhere if needed
                }
            }
        }

        // Method to archive the customer record
        private async Task ArchiveCustomer(Customer customer)
        {
            try
            {
                // Create a new archive record
                var archivedCustomer = new ArchiveCustomer
                {
                    StudentId = customer.StudentId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    CampusName = customer.CampusName,
                    PhoneNumber = customer.PhoneNumber,
                    EmergencyContactName = customer.EmergencyContactName,
                    EmergencyContactNum = customer.EmergencyContactNum,
                    TypeOfCustomer = customer.TypeOfCustomer,
                    TypeOfRider = customer.TypeOfRider,
                    TAndCAgreement = customer.TAndCAgreement,
                    EmailSubscription = customer.EmailSubscription,
                    ArchivedOn = DateTime.Now // Set the archived date
                };

                // Add the archive record to the archive table
                dbContext.ArchiveCustomer.Add(archivedCustomer);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements
                Console.WriteLine($"Failed to archive customer: {ex.Message}");
                throw; // Re-throw the exception to be handled elsewhere if needed
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminList()
        {
            var customers = await dbContext.CustomerInformation.ToListAsync();
            return View(customers);



        }
        [HttpPost]
        public async Task<IActionResult> RetrieveCustomer(int studentId)
        {
            var archivedCustomer = await dbContext.ArchiveCustomer.FirstOrDefaultAsync(x => x.StudentId == studentId);
            if (archivedCustomer != null)
            {
                // Add the archived customer back to the CustomerInformation table
                var customer = new Customer
                {
                    StudentId = archivedCustomer.StudentId,
                    FirstName = archivedCustomer.FirstName,
                    LastName = archivedCustomer.LastName,
                    Email = archivedCustomer.Email,
                    CampusName = archivedCustomer.CampusName,
                    PhoneNumber=archivedCustomer.PhoneNumber,
                    EmergencyContactName = archivedCustomer.EmergencyContactName,
                    EmergencyContactNum = archivedCustomer.EmergencyContactNum,
                    TypeOfCustomer = archivedCustomer.TypeOfCustomer,
                    TypeOfRider = archivedCustomer.TypeOfRider,
                    TAndCAgreement  = archivedCustomer.TAndCAgreement,
                    EmailSubscription = archivedCustomer.EmailSubscription,
                };

                dbContext.CustomerInformation.Add(customer);
                await dbContext.SaveChangesAsync();

                // Now delete the archived record
                dbContext.ArchiveCustomer.Remove(archivedCustomer);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("AdminList", "Customer"); // Redirect back to the archived customers view
            }

            return NotFound(); // Handle case where archived customer is not found
        }

        public async Task<IActionResult> ArchivedCustomers()
        {
            var archivedCustomers = await dbContext.ArchiveCustomer.ToListAsync();
            return View(archivedCustomers);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteArchivedCustomer(int id)
        {
            var archivedCustomer = await dbContext.ArchiveCustomer.FirstOrDefaultAsync(x => x.StudentId == id);
            if (archivedCustomer != null)
            {
                dbContext.ArchiveCustomer.Remove(archivedCustomer);
                await dbContext.SaveChangesAsync();
                return Ok(); // Return success status if deletion is successful
            }
            return NotFound(); // Return not found status if archived customer not found
        }

        // Navigate to the admin dashboard.
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

    }
}
