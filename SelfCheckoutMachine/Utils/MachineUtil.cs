using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Migrations;

namespace SelfCheckoutMachine.Utils
{
    public static class MachineUtil
    {
        public static JsonResult ErrorHandlerForInsertedObject(JsonElement body, out JsonElement inserted, SelfCheckoutMachineContext context)
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

            bool billIsNotSupported = insertedBills.Any(ib => context.Bills.All(b => b.BillName != ib.Name));
            if (billIsNotSupported)
            {
                var unsupportedBills = insertedBills.Where(ib => context.Bills.All(b => b.BillName != ib.Name));
                var messageParameter = String.Join(',', unsupportedBills.Select(ub => ub.Name).ToArray());

                return new JsonResult(new { StatusCode = 422, Value = $"The following bill type is not supported by the machine: {messageParameter}" });
            }

            return null;
        }

        public static JsonResult ErrorHandlerForPriceObject(JsonElement body, out JsonElement price, SelfCheckoutMachine context)
        {
            throw new NotImplementedException();
        }
    }
}
