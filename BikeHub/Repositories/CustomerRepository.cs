using BikeHub.Data;
using BikeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeHub.Repositories
{
    /*
    * Customer Repo:
    * Add filters for generating customer reports
    * @Author: Rawah Almesri
    * Date: Nov 29, 2024
    */
    public class CustomerRepository
    {
        private readonly BikeHubDBContext _context;

        public CustomerRepository(BikeHubDBContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetFilteredCustomersAsync(
            string? fname,
            string?lname,
            string? email,
            string? campus,
            string? typeofcus
           
           
           )
        {
            // Start with the base query
            var query = _context.CustomerInformation.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(fname))
                query = query.Where(c => c.FirstName.Contains(fname));
            
            if (!string.IsNullOrEmpty(lname))
                query = query.Where(c => c.LastName.Contains(lname));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(c => c.Email.Contains(email));

            if (!string.IsNullOrEmpty(campus))
                query = query.Where(c => c.CampusName == campus);
            if (!string.IsNullOrEmpty(typeofcus))
                query = query.Where(c => c.TypeOfCustomer == typeofcus);

            // Return the filtered list
            return await query.ToListAsync();
        }
    }
}
