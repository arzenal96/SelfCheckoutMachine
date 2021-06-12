using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Entities;
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
    public class StockController : ControllerBase
    {
        private SelfCheckoutMachineContext _context;

        public StockController(SelfCheckoutMachineContext Context)
        {
            _context = Context;
        }

        [HttpGet]
        public ObjectResult Get([FromBody] JsonElement body)
        {
            JsonElement inserted;

            var objectResult = MachineUtil.ErrorHandlerForInsertedObject(body, out inserted, _context);
            if (objectResult != null)
            {
                return objectResult;
            }

            return new ObjectResult(new { StatusCode = 200, Value =  body.GetProperty("inserted") });
        }

        [HttpPost]
        public ObjectResult Post([FromBody] JsonElement body)
        {
            JsonElement inserted;

            var objectResult = MachineUtil.ErrorHandlerForInsertedObject(body, out inserted, _context);
            if (objectResult != null)
            {
                return objectResult;
            }

            foreach (var bill in inserted.EnumerateObject())
            {
                var billId = _context.Bills.First(b => b.BillName == bill.Name).Id;
                var amount = int.Parse(bill.Value.ToString());

                var billType = _context.Stocks.FirstOrDefault(s => s.BillId == billId);
                if (billType != null)
                {
                    billType.Amount += amount;
                    _context.Update(billType);
                }
                else
                {
                    _context.Stocks.Add(new Stock
                    {
                        BillId = billId,
                        Amount = amount
                    });
                }


                _context.SaveChanges();
            }

            return new ObjectResult(new { StatusCode = 200, Value = body.GetProperty("inserted") });
        }
    }
}
