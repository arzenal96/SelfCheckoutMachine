using Microsoft.EntityFrameworkCore;
using SelfCheckoutMachine.Entities;
using SelfCheckoutMachine.Interfaces;

namespace SelfCheckoutMachine.Migrations
{
    public class SelfCheckoutMachineContext : DbContext
    {
        public DbSet<Bill> Bills { get; private set; }
        public DbSet<Stock> Stocks { get; private set; }

        public SelfCheckoutMachineContext(DbContextOptions<SelfCheckoutMachineContext> options)
            : base(options)
        {

        }
    }
}
