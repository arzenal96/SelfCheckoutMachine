using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Migrations;
using SelfCheckoutMachine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SelfCheckoutMachine.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private SelfCheckoutMachineContext _context;

        public CheckoutController(SelfCheckoutMachineContext Context)
        {
            _context = Context;
        }

        [HttpPost]
        public ObjectResult Post([FromBody] JsonElement body)
        {
            Console.WriteLine("/api/v1/Checkout POST");
            JsonElement inserted, price;

            var objectResult = MachineUtil.ErrorHandlerForInsertedObject(body, out inserted, _context);
            Console.WriteLine($"Inserted: {inserted}");

            if (objectResult != null)
            {
                return objectResult;
            }

            objectResult = MachineUtil.ErrorHandlerForPriceObject(body, out price);
            Console.WriteLine($"Price: {price}");
            if (objectResult != null)
            {
                return objectResult;
            }

            var paid = 0;

            foreach (var bill in inserted.EnumerateObject())
            {
                var billNameIntValue = int.Parse(bill.Name.ToString());
                var billAmount = int.Parse(bill.Value.ToString());
                paid += billNameIntValue * billAmount;
            }

            Console.WriteLine($"Paid: {paid}");

            var remainder = paid - int.Parse(price.ToString());
            Console.WriteLine($"Remainder: {remainder}");

            var dictionary = new Dictionary<string, int>();

            if (remainder < 0)
            {
                return new ObjectResult($"You need to pay {Math.Abs(remainder)} more for the product.") { StatusCode = 400 };
            }
            else if (remainder == 0)
            {
                return new ObjectResult("Successful purchase!") { StatusCode = 200 };
            }
            else
            {
                return CalculateRemainder(remainder, dictionary);
            }
        }

        private ObjectResult CalculateRemainder(int remainder, Dictionary<string, int> dictionary)
        {
            var availableBills = _context.Stocks.Join(_context.Bills, s => s.BillId, b => b.Id, (s, b) => new
            {
                s.BillId,
                BillValue = Convert.ToInt32(b.BillName),
                BillAmount = s.Amount
            }).OrderByDescending(ab => ab.BillValue);

            var sum = 0;
            foreach (var bills in availableBills)
            {
                sum += bills.BillValue * bills.BillAmount;
            }

            if (sum < remainder)
            {
                return new ObjectResult("There's not enough money to pay the remainder.") { StatusCode = 400 };
            }
            else
            {
                foreach (var bill in availableBills)
                {
                    int wholePart = (int)Math.Floor((double)(remainder / bill.BillValue));
                    if (wholePart > 0)
                    {
                        remainder -= bill.BillValue * wholePart;

                        var stock = _context.Stocks.FirstOrDefault(s => s.BillId == bill.BillId);
                        stock.Amount -= wholePart;

                        _context.Update(stock);

                        dictionary.Add(bill.BillValue.ToString(), wholePart);
                    }
                }

                _context.SaveChanges();
                Console.WriteLine("Data saved!");

                return new ObjectResult(dictionary) { StatusCode = 200 };
            }
        }

    }
}
