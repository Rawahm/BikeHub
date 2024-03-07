using BikeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeHub.Data
{
    public class BikeHubDBContext : DbContext
    {
        public BikeHubDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> CustomerDetails { get; set; }
    }
}