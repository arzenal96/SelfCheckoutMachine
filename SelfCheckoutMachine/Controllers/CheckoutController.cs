using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Entities;
using SelfCheckoutMachine.Migrations;
using System;
using System.Collections.Generic;
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
