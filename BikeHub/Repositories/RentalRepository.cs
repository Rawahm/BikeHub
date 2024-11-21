using BikeHub.Data;
using BikeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeHub.Repositories
{
    /*
 * Rental Repo:
 * Add filters 
 * @Author:Rawah Almesri
 * Date: Nov 14,2024
 */
    public class RentalRepository
    {
        private readonly BikeHubDBContext _context;

        public RentalRepository(BikeHubDBContext context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetFilteredRentalsAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? customerName,
            string? bikeRented,
            string? availabilityStatus,
            bool? paid,
            bool? overdue)
        {
            // Start with the base query
            var query = _context.Rental.Include(r => r.Customer).AsQueryable();

            // Apply filters
            if (startDate.HasValue)
                query = query.Where(r => r.DateRented >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.DateRented <= endDate.Value);
            
            // filter by name
            if (!string.IsNullOrEmpty(customerName))
                query = query.Where(r => r.Name.Contains(customerName));

            // filter by the bike  type/name
            if (!string.IsNullOrEmpty(bikeRented))
                query = query.Where(r => r.BikeRented == bikeRented);

            // By avaliablity
            if (!string.IsNullOrEmpty(availabilityStatus))
                query = query.Where(r => r.AvailabilityStatus == availabilityStatus);

            // Paid yes-no or all
            if (paid.HasValue)
                query = query.Where(r => r.Paid == paid.Value);

            // Filter by overdue status (compare DueDate and DateReturned)
            if (overdue.HasValue)
            {
                var currentDate = DateTime.Now;

                if (overdue.Value)
                {
                    // Overdue: If DateReturned is null (not yet returned) or DateReturned is later than DueDate
                    query = query.Where(r =>
                        r.DateReturned == null || r.DateReturned > r.DueDate);
                }
                else
                {
                    // Not overdue: returned on time or not due yet
                    query = query.Where(r => r.DateReturned <= r.DueDate);
                }
            }


            // Show and return the results - apply all above queries
            return await query.ToListAsync();
        }
    }
}
