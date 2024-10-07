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
        // GET: Inventory/Index
        public IActionResult Index()
        {
            var inventoryList = dbContext.InventoryTable.ToList(); // Select all inventory items
            return View(inventoryList);
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
                    RetiredOptions = model.AvailabilityStatus == "Retired" ? model.RetiredOptions : null  // Only set RetiredOptions if retired
                };

                await dbContext.InventoryTable.AddAsync(inventory);
                try
                {
                    await dbContext.SaveChangesAsync();
                    TempData["SuccessMessage"] = "New inventory item added successfully.";
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving inventory: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Inventory/UpdateInventory/{id}
        public async Task<IActionResult> UpdateInventory(int id)
        {
            var inventoryItem = await dbContext.InventoryTable.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            var model = new InventoryViewModel
            {
                ItemId = inventoryItem.ItemId,
                ItemType = inventoryItem.ItemType,
                ItemNumber = inventoryItem.ItemNumber,
                Condition = inventoryItem.Condition,
                Price = inventoryItem.Price,
                AvailabilityStatus = inventoryItem.AvailabilityStatus,
                Comments = inventoryItem.Comments,
                RetiredOptions = inventoryItem.RetiredOptions
            };

            return View(model);
        }

        // POST: Inventory/UpdateInventory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInventory(InventoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var inventoryItem = await dbContext.InventoryTable.FindAsync(model.ItemId);
                if (inventoryItem == null)
                {
                    return NotFound();
                }

                inventoryItem.ItemType = model.ItemType;
                inventoryItem.ItemNumber = model.ItemNumber;
                inventoryItem.Condition = model.Condition;
                inventoryItem.Price = model.Price;
                inventoryItem.AvailabilityStatus = model.AvailabilityStatus;
                inventoryItem.Comments = model.Comments;

                // Only set RetiredOptions if the AvailabilityStatus is "Retired"
                if (model.AvailabilityStatus == "Retired")
                {
                    inventoryItem.RetiredOptions = model.RetiredOptions;
                }
                else
                {
                    inventoryItem.RetiredOptions = null; // Clear RetiredOptions if not retired
                }

                try
                {
                    await dbContext.SaveChangesAsync();
                    //  TempData["SuccessMessage"] = "Inventory item updated successfully.";
                    return RedirectToAction("UpdateSuccess");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating inventory: " + ex.Message);
                }
            }

            return View(model);

        }

        // GET: Inventory/UpdateSuccess
        public IActionResult UpdateSuccess()
        {
            return View();
        }
        // GET: Inventory/DeleteInventory/{id}
            
        [HttpPost]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var inventoryItem = await dbContext.ArchivedInventory.FindAsync(id);
            if (inventoryItem != null)
            {
                dbContext.ArchivedInventory.Remove(inventoryItem);
                await dbContext.SaveChangesAsync();
            }
            return Ok(); // ** in the sript side it will upload the page with the new view taht does not include the deleted record  **
        }






        public async Task<IActionResult> ArchiveInventory(int id)
        {
            var inventoryItem = await dbContext.InventoryTable.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            // Create an archived item
            var archivedItem = new ArchivedInventory
            {
                ItemId = inventoryItem.ItemId,
                ItemType = inventoryItem.ItemType,
                ItemNumber = inventoryItem.ItemNumber,
                Condition = inventoryItem.Condition,
                Price = inventoryItem.Price,
                AvailabilityStatus = inventoryItem.AvailabilityStatus,
                Comments = inventoryItem.Comments,
                RetiredOptions = inventoryItem.RetiredOptions,
                ArchivedDate = DateTime.Now

                //  ArchivedDate = inventoryItem.ArchivedDate
            };

            // Remove from the main inventory table
            dbContext.InventoryTable.Remove(inventoryItem);

            // Add to the archived inventory table
            await dbContext.ArchivedInventory.AddAsync(archivedItem);

            await dbContext.SaveChangesAsync();
            return RedirectToAction("ArchivedItems"); // Redirect after archiving
        }

        // GET: Inventory/ArchivedItems
        public IActionResult ArchivedItems()
        {
            var archivedItems = dbContext.ArchivedInventory.ToList(); // Fetch archived items
            return View(archivedItems);
        }
        public IActionResult Success()
        {
            // Retrieve the success message from TempData
            var successMessage = TempData["SuccessMessage"] as string;
            return View("Success", successMessage); // Pass the message to the view
        }
    }
}