using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Migrations;

namespace SelfCheckoutMachine.Utils
{
    public static class MachineUtil
    {
        public static ObjectResult ErrorHandlerForInsertedObject(JsonElement body, out JsonElement inserted, SelfCheckoutMachineContext context)
        {
            if (!body.TryGetProperty("inserted", out inserted))
            {
                return new ObjectResult("Missing request entity: inserted") { StatusCode = 422 };
            }

            var insertedBills = inserted.EnumerateObject();

            bool billQuantityIsNotAnInteger = insertedBills.Any(ib => !int.TryParse(ib.Value.ToString(), out _));
            if (billQuantityIsNotAnInteger)
            {
                var invalidBills = insertedBills.Where(ib => !int.TryParse(ib.Value.ToString(), out _));
                var messageParameter = String.Join(',', invalidBills.Select(ib => ib.Name).ToArray());

                return new ObjectResult(new { Value = $"The following bills type's value is invalid, it must be an integer: {messageParameter}" }) { StatusCode = 422 };
            }

            bool billIsNotSupported = insertedBills.Any(ib => context.Bills.All(b => b.BillName != ib.Name));
            if (billIsNotSupported)
            {
                var unsupportedBills = insertedBills.Where(ib => context.Bills.All(b => b.BillName != ib.Name));
                var messageParameter = String.Join(',', unsupportedBills.Select(ub => ub.Name).ToArray());

                return new ObjectResult($"The following bill type is not supported by the machine: {messageParameter}") { StatusCode = 422 };
            }

            return null;
        }

        public static ObjectResult ErrorHandlerForPriceObject(JsonElement body, out JsonElement price)
        {
            if (!body.TryGetProperty("price", out price))
            {
                return new ObjectResult("Missing request entity: price") { StatusCode = 422 };
            }

            if (!int.TryParse(price.ToString(), out _))
            {
                return new ObjectResult("Price's type is invalid, it must be a number") { StatusCode = 422 };
            }

            return null;
        }
    }
}
