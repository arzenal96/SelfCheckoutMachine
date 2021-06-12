using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Entities;
using SelfCheckoutMachine.Migrations;
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
        public JsonResult Get([FromBody] JsonElement body)
        {
            JsonElement inserted;

            var jsonResult = ErrorHandler(body, out inserted);
            if (jsonResult != null)
            {
                return jsonResult;
            }

            return new JsonResult(new { StatusCode = 200, Value =  body.GetProperty("inserted") });
        }

        [HttpPost]
        public JsonResult Post([FromBody] JsonElement body)
        {
            JsonElement inserted;

            var jsonResult = ErrorHandler(body, out inserted);
            if (jsonResult != null)
            {
                return jsonResult;
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

            return new JsonResult(new { StatusCode = 200, Value = body.GetProperty("inserted") });
        }

        private JsonResult ErrorHandler(JsonElement body, out JsonElement inserted)
        {
            if (!body.TryGetProperty("inserted", out inserted))
            {
                return new JsonResult(new { StatusCode = 422, Value = "Missing request entity: \"inserted\"" });
            }

            var insertedBills = inserted.EnumerateObject();

            bool billQuantityIsNotAnInteger = insertedBills.Any(ib => !int.TryParse(ib.Value.ToString(), out _));
            if (billQuantityIsNotAnInteger)
            {
                var invalidBills = insertedBills.Where(ib => !int.TryParse(ib.Value.ToString(), out _));
                var messageParameter = String.Join(',', invalidBills.Select(ib => ib.Name).ToArray());

                return new JsonResult(new { StatusCode = 422, Value = $"The following bills type's value is invalid, it must be a number: {messageParameter}" });
            }

            bool billIsNotSupported = insertedBills.Any(ib => _context.Bills.All(b => b.BillName != ib.Name));
            if (billIsNotSupported)
            {
                var unsupportedBills = insertedBills.Where(ib => _context.Bills.All(b => b.BillName != ib.Name));
                var messageParameter = String.Join(',', unsupportedBills.Select(ub => ub.Name).ToArray());

                return new JsonResult(new { StatusCode = 422, Value = $"The following bill type is not supported by the machine: {messageParameter}" });
            }

            return null;
        }
    }
}
