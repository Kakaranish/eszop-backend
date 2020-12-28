using Microsoft.EntityFrameworkCore;

namespace Orders.API.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
