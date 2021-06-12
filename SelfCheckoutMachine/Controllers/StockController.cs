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
    public class StockController : ControllerBase
    {
        private SelfCheckoutMachineContext _context;

        public StockController(SelfCheckoutMachineContext Context)
        {
            _context = Context;
        }

        [HttpGet]
        public IEnumerable<Stock> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IEnumerable<Stock> Post([FromBody] JsonElement body)
        {
            throw new NotImplementedException();
        }
    }
}
