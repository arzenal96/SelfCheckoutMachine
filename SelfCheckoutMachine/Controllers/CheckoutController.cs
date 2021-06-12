using Microsoft.AspNetCore.Mvc;
using SelfCheckoutMachine.Entities;
using System;
using System.Collections.Generic;
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
        public IEnumerable<Checkout> Post()
        {
            throw new NotImplementedException();
        }
    }
}
