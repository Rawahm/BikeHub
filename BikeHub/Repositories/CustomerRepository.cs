using BikeHub.Data;

namespace BikeHub.Repositories
{
    public class CustomerRepository
    {
        private readonly BikeHubDBContext _context;


        public CustomerRepository(BikeHubDBContext context)
        {
            _context = context;
        }

    }
}