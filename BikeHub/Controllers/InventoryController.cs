using BikeHub.Data;
using BikeHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace BikeHub.Controllers
{

   /*
    * @Author , Date
    * Created By: Rawah Almesri 
    *  Sat Oct 5,2024
    */
    public class InventoryController : Controller
    {
        private readonly BikeHubDBContext dbContext;

        public InventoryController(BikeHubDBContext dbContext)
        {
            this.dbContext = dbContext;

        }
        // /Inventory
        public IActionResult Index()
        {
            var inventoryList = dbContext.InventoryTable.ToList(); // Select  all inventory items from the database
            return View(inventoryList);
            // return View();
        }
        // GET: Inventory/CreateInventory
        public IActionResult CreateInventory()
        {
            return View();
        }

        // POST: Inventory/CreateInventory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInventory(InventoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var inventory = new Inventory
                {
                    ItemId = model.ItemId,
                    ItemType = model.ItemType,
                    ItemNumber = model.ItemNumber,
                    Condition = model.Condition,
                    Price = model.Price,
                    AvailabilityStatus = model.AvailabilityStatus,
                    Comments = model.Comments,
                    RetiredOptions = model.RetiredOptions

                };

                await dbContext.InventoryTable.AddAsync(inventory);

                try
                {
                    await dbContext.SaveChangesAsync();  // Use await here
                    TempData["SuccessMessage"] = "New inventory item added successfully."; // Set success message
                    return RedirectToAction("Success");  // Redirect to success page
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving inventory: " + ex.Message);
                }
            }

            return View(model);
        }

        public IActionResult Success()
        {
             var successMessage = TempData["SuccessMessage"] as string;
            return View("Success", successMessage); // Pass the message to the view
        }


    }
}
