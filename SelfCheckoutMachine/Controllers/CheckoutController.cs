using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SelfCheckoutMachine.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CheckoutController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Checkout> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IEnumerable<Checkout> Post([FromBody] JsonElement body)
        {
            throw new NotImplementedException();
        }
    }
}
