using BikeHub.Data;
using BikeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
/*
 * @Author : Rawah Almesri 
 * 2024-03-21
 */


namespace BikeHub.Controllers
{
    [Authorize]

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
                    MethodOfTravel = model.MethodOfTravel,
                    PaymentMethod = model.PaymentMethod,
                    AvailabilityStatus = model.AvailabilityStatus
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

        // the first version of the create method..

        /*
       public async Task<IActionResult> Create(RentalViewModel model)
{
    // Check if the model state is valid
    if (!ModelState.IsValid)
    {
        // If the model state is not valid, return the view with validation errors
        return View(model);
    }

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
        [HttpGet]
        public IActionResult ViewRentedBikes()
        {
            // Show all  rentals where AvailabilityStatus
            // is equal to "Rented"
            var rentedBikes = dbContext.Rental
                .Where(r => r.AvailabilityStatus == "Rented")
                .Select(r => new Rental
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    Name = r.Name,
                    Email = r.Email,
                    BikeRented = r.BikeRented,
                    DateRented = r.DateRented,
                    DueDate = r.DueDate,
                    Amount = r.Amount,
                    Paid = r.Paid,
                    AvailabilityStatus = r.AvailabilityStatus
                })
                .ToList();

            return View(rentedBikes); // Pass rented bikes to the view
        }
        // GET: /Rental/Edit/Id
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Fetch the rental record by ID
            var rental = dbContext.Rental.FirstOrDefault(r => r.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            // Convert the rental entity to the RentalViewModel
            var viewModel = new RentalViewModel
            {
                Id = rental.Id,
                StudentId = rental.StudentId,
                Name = rental.Name,
                Email = rental.Email,
                BikeRented = rental.BikeRented,
                AvailabilityStatus = rental.AvailabilityStatus,
                PaymentMethod = rental.PaymentMethod,
                DateRented = rental.DateRented,
                DueDate = rental.DueDate,
                LockRented = rental.LockRented,
                BasketRented = rental.BasketRented,
                KeyRented = rental.KeyRented,
                Lights = rental.Lights,
                Duration = rental.Duration,
                Amount = rental.Amount,
                KMSRidden = rental.KMSRidden,
                MethodOfTravel = rental.MethodOfTravel,
                PaymentDate = rental.PaymentDate,
                DateReturned = rental.DateReturned,
                DaysLate = rental.DaysLate,
                CampusName = rental.CampusName,
                Banned = rental.Banned,
                Paid = rental.Paid,
                Notes = rental.Notes
            };

            return View(viewModel);
        }
        // Rental/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(RentalViewModel rentalViewModel)
        {
            var rental = await dbContext.Rental.FindAsync(rentalViewModel.Id);
            if(rental is not null)
            { 
            


                // Update the rental record
                // rental.StudentId = model.StudentId;
                // rental.Name = model.Name;
                rental.Email = rentalViewModel.Email;
                rental.BikeRented = rentalViewModel.BikeRented;
                rental.AvailabilityStatus = rentalViewModel.AvailabilityStatus;
                rental.PaymentMethod = rentalViewModel.PaymentMethod;
                rental.DateRented = rentalViewModel.DateRented;
                rental.DueDate = rentalViewModel.DueDate;
                rental.LockRented = rentalViewModel.LockRented;
                rental.BasketRented = rentalViewModel.BasketRented;
                rental.KeyRented = rentalViewModel.KeyRented;
                rental.Lights = rentalViewModel.Lights;
                rental.Duration = rentalViewModel.Duration;
                rental.Amount = rentalViewModel.Amount;
                rental.KMSRidden = rentalViewModel.KMSRidden;
                rental.MethodOfTravel = rentalViewModel.MethodOfTravel;
                rental.PaymentDate = rentalViewModel.PaymentDate;
                rental.DateReturned = rentalViewModel.DateReturned;
                rental.DaysLate = rentalViewModel.DaysLate;
                rental.CampusName = rentalViewModel.CampusName;
                rental.Banned = rentalViewModel.Banned;
                rental.Paid = rentalViewModel.Paid;
                rental.Notes = rentalViewModel.Notes;
                
                // Save the changes to the database!!

                await dbContext.SaveChangesAsync();
                // Testing
                Console.WriteLine("Saving changes.!!..");
              
                // Redirect back to the rental list -- All lists of rental!!!
                return RedirectToAction(nameof(Index));
            }

            // If the model is not valid, return the view with the same data-- Not working! 
            return View(rentalViewModel);
        }
    }
}
