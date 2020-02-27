using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Tryout2.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {


        private readonly ILogger<OrderController> _logger;

        private readonly OnlineOrderContext _context;

        public OrderController(ILogger<OrderController> logger, OnlineOrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOrder()
        {
            var Cust = _context.Order;
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int Id)
        {
            var Cust = _context.Order.Find(Id);
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpPost]
        public IActionResult OrderAdd()
        {
            var custi = new Orders
            {
                user_id = 1,
                order_status = "ok",
                driver_id = 1,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Order.Add(custi);
            _context.SaveChanges();
            return Ok(new { message = "success post data", status = true, data = custi });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int Id)
        {
            var Cust = _context.Order.Find(Id);
            _context.Order.Remove(Cust);
            _context.SaveChangesAsync();

            return Ok(Cust);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAuthor([FromBody]JsonPatchDocument<Orders> patch, int Id)
        {

            patch.ApplyTo(_context.Driver.Find(Id));

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult PutOrder([FromBody]JsonPatchDocument<Orders> patch, int Id)
        {

            var Cust = _context.Orders.First(a => a.id == Id);
            Cust.order_status = "not ok";
            Cust.updated_at = DateTime.Now;
            _context.SaveChangesAsync();

            patch.ApplyTo(_context.Orders.Find(Id));

            return Ok(new { message = "success put data", status = true, data = Cust } );
        }
    }
}
