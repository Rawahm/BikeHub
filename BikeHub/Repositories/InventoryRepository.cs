using BikeHub.Data;
using BikeHub.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace BikeHub.Repositories
{
    public class InventoryRepository
    {

        private readonly BikeHubDBContext _context;

        public InventoryRepository(BikeHubDBContext context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetFilteredInventoryAsync(
            string? itemType,
            string? itemNum,
            string? condition,
            decimal? price,
            string? availabilityStatus,
            string? retiredOptions


           )
        {
            // Start with the base query
            var query = _context.InventoryTable.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(itemType))
                query = query.Where(c => c.ItemType.Contains(itemType));

            if (!string.IsNullOrEmpty(itemNum))
                query = query.Where(c => c.ItemNumber.Contains(itemNum));

            if (!string.IsNullOrEmpty(condition))
                query = query.Where(c => c.Condition.Contains(condition));

            if (price > 0)
                query = query.Where(c => c.Price == price);
           
            if (!string.IsNullOrEmpty(availabilityStatus))
                query = query.Where(c => c.AvailabilityStatus == availabilityStatus);

            // Return the filtered list
            return await query.ToListAsync();
        }
    }
}
