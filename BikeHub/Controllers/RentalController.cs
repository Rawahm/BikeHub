using BikeHub.Data;
using BikeHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
/*
 * @Author : Rawah Almesri 
 * 2024-03-21
 */


namespace BikeHub.Controllers
{
    public class RentalController : Controller
    {
        private readonly BikeHubDBContext dbContext;

        public RentalController(BikeHubDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]

        public IActionResult Create(int customerId)
        {
            ///<summary>
            ///Retrieve customer information based on customer ID
            ///Inside this page Admin can create one or more Rental.
            /// </summary>

            var customer = dbContext.CustomerInformation.FirstOrDefault(c => c.StudentId == customerId);
            if (customer == null)
            {
                // Handle case where customer is not found
                return NotFound();
            }

            // Pass customer information to the rental creation view
            var rentalViewModel = new RentalViewModel
            {
                Customer = customer,
                Name = $"{customer.FirstName} {customer.LastName}",
                Email = customer.Email
            };
            return View(rentalViewModel);
        }
        //This the method shown during the sprint 2. 
         // This create method  added to validate the saving of the records Another commented create method
         // without any form validation 
        public async Task<IActionResult> Create(RentalViewModel model)
        {
            try
            {
                // Retrieve the customer from the database using the provided CustomerId
                var customer = await dbContext.CustomerInformation.FindAsync(model.Customer.StudentId);
                if (customer == null)
                {
                    ModelState.AddModelError("", "Customer not found.");
                    return View(model);
                }

                // Create a new Rental entity and populate its properties
                var rental = new Rental
                {
                    // Set the Customer property to the retrieved customer
                    Customer = customer,
                    StudentId = model.Customer.StudentId,
                    Name = model.Name,
                    Email = model.Email,
                    BikeRented = model.BikeRented,
                    Lights = model.Lights,
                    DateRented = model.DateRented,
                    DateReturned = model.DateReturned,
                    KeyRented = model.KeyRented,
                    DaysLate = model.DaysLate,
                    DueDate = model.DueDate,
                    Duration = model.Duration,
                    Amount = model.Amount,
                    Banned = model.Banned,
                    BasketRented = model.BasketRented,
                    Paid = model.Paid,
                    PaymentDate = model.PaymentDate,
                    KMSRidden = model.KMSRidden,
                    LockRented = model.LockRented,
                    Notes = model.Notes,
                    MethodOfTravel = model.MethodOfTravel
                };

                // Add the rental to the context and save changes
                dbContext.Rental.Add(rental);
                await dbContext.SaveChangesAsync();

                // Redirect to the Index page : /Rental/Index with the Get request.....
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ModelState.AddModelError("", "An error occurred while saving the rental information.");
                return View(model);
            }
        }

         // End of the first version of the create method..

        /*
        public async Task<IActionResult> Create(RentalViewModel model)
        {
            // Retrieve the customer from the database using the provided CustomerId
           // var customer = await dbContext.CustomerInformation.FindAsync(keyValues: model.Customer.StudentId);

            // Create a new Rental entity and populate its properties
            var rental = new Rental
            {
                // Set the Customer property to the retrieved customer
              //  Customer = customer,
              StudentId = model.Customer.StudentId,
                Name = model.Customer.FirstName + "" + model.Customer.LastName,
                Email = model.Customer.Email,
                BikeRented = model.BikeRented,
                Lights = model.Lights,
                DateRented = (DateTime)model.DateRented,
                DateReturned = (DateTime)model.DateReturned,
                KeyRented = model.KeyRented,
                DaysLate = model.DaysLate,
                DueDate = (DateTime)model.DueDate,
                Duration = model.Duration,
                Amount = model.Amount,
                Banned = model.Banned,
                BasketRented = model.BasketRented,
                Paid = model.Paid,
                PaymentDate = model.PaymentDate,
                KMSRidden = model.KMSRidden,
                LockRented = model.LockRented,
                Notes = model.Notes,
                MethodOfTravel = model.MethodOfTravel
            };
            // Add the rental to the context and save changes
            dbContext.Rental.Add(rental);
            await dbContext.SaveChangesAsync();

            // Redirect to the Index page : /Rental/Index with the Get request.....
            return RedirectToAction("Index");
        }
        */
        [HttpGet]
        public IActionResult Index()
        {
            var rentals = dbContext.Rental.ToList();
            return View(rentals);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int customerId)
        {
            var customerWithRental = await dbContext.CustomerInformation
                .Include(c => c.RentalDetails) // Include rentals for the customer
                .FirstOrDefaultAsync(c => c.StudentId == customerId);

            if (customerWithRental == null)
            {
                return NotFound();
            }

            var viewModel = new CustomerRentalViewModel
            {
                Customer = customerWithRental,
                Rentals = customerWithRental.RentalDetails.ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRental(int id)
        {
            var rental = await dbContext.Rental.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            // Store the customer ID associated with this rental

            ViewBag.CustomerId = rental.StudentId;



            dbContext.Rental.Remove(rental);
            await dbContext.SaveChangesAsync();
            // Redirect to the details page after deletion
            return View();
        }
        // When delte the rental record redirect to success page 
        [HttpGet]
        public IActionResult DeleteSuccess()
        {
            return View();
        }

    }
}
