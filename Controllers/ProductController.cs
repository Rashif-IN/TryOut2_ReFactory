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
    [Route("product")]
    public class ProductController : ControllerBase
    {


        private readonly ILogger<ProductController> _logger;

        private readonly OnlineOrderContext _context;

        public ProductController(ILogger<ProductController> logger, OnlineOrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProduct()
        {
            var Cust = _context.Product;
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int Id)
        {
            var Cust = _context.Product.Find(Id);
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpPost]
        public IActionResult ProductAdd()
        {
            var custi = new Products
            {
                name = "stand arrow",
                price = 9999999,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Product.Add(custi);
            _context.SaveChanges();
            return Ok(new { message = "success post data", status = true, data = custi });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int Id)
        {
            var Cust = _context.Product.Find(Id);
            _context.Product.Remove(Cust);
            _context.SaveChangesAsync();

            return Ok();
        }

       

        
        [HttpPut("{id}")]
        public IActionResult PutCustomer([FromBody]JsonPatchDocument<Products> patch, int Id)
        {

            var Cust = _context.Products.First(a => a.id == Id);
            Cust.name = "Requiem Arrow";
            Cust.updated_at = DateTime.Now;
            _context.SaveChangesAsync();

            patch.ApplyTo(_context.Products.Find(Id));

            return Ok(new { message = "success put data", status = true, data = Cust } );
        }
    }
}
