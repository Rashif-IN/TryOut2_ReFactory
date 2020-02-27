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
    [Route("orderitem")]
    public class OrderItemController : ControllerBase
    {


        private readonly ILogger<OrderItemController> _logger;

        private readonly OnlineOrderContext _context;

        public OrderItemController(ILogger<OrderItemController> logger, OnlineOrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOrder()
        {
            var Cust = _context.Order_Item;
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int Id)
        {
            var Cust = _context.Order_Item.Find(Id);
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpPost]
        public IActionResult OrderAdd()
        {
            var custi = new Order_items
            {
                order_id = 1,
                product_id = 1,
                quantity = 1
            };

            _context.Order_Item.Add(custi);
            _context.SaveChanges();
            return Ok(new { message = "success post data", status = true, data = custi });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int Id)
        {
            var Cust = _context.Order_Item.Find(Id);
            _context.Order_Item.Remove(Cust);
            _context.SaveChangesAsync();

            return Ok(Cust);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAuthor([FromBody]JsonPatchDocument<Order_items> patch, int Id)
        {

            patch.ApplyTo(_context.Order_Item.Find(Id));

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult PutOrderItem([FromBody]JsonPatchDocument<Order_items> patch, int Id)
        {

            var Cust = _context.Order_items.First(a => a.id == Id);
            Cust.quantity = 2;
            _context.SaveChangesAsync();

            patch.ApplyTo(_context.Order_items.Find(Id));

            return Ok(new { message = "success put data", status = true, data = Cust } );
        }
    }
}
