using Microsoft.EntityFrameworkCore;

namespace SelfCheckoutMachine.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public string BillName { get; set; }
        public int Amount { get; set; }
    }
}
