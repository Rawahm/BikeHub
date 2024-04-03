using BikeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeHub.Data
{
    public class BikeHubDBContext : DbContext
    {
        public BikeHubDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> CustomerInformation { get; set; }
        public DbSet<Rental> Rental { get; set;}
        public DbSet<ArchiveCustomer> ArchiveCustomer { get; set; } // Add this DbSet for the archive table


    }
}