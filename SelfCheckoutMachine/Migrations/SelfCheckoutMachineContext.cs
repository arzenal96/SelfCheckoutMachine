using Microsoft.EntityFrameworkCore;
using SelfCheckoutMachine.Entities;

namespace SelfCheckoutMachine.Migrations
{
    public class SelfCheckoutMachineContext : DbContext
    {
        public DbSet<Stock> Stocks { get; private set; }

        public SelfCheckoutMachineContext(DbContextOptions<SelfCheckoutMachineContext> options)
            : base(options)
        {

        }
    }
}
