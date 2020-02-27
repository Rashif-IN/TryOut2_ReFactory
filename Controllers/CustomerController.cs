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
    [HelpOption("--hlp")]
    [Subcommand(
        //1
        typeof(List_),
        typeof(Add_),
        //typeof(Update_),
        typeof(Delete_)
    )]

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return CommandLineApplication.Execute<Program>(args);
        }
    }

    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {

        
        private readonly ILogger<CustomerController> _logger;

        private readonly OnlineOrderContext _context;

        public CustomerController(ILogger<CustomerController> logger, OnlineOrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCustomer()
        {
            var Cust = _context.Customer;
            return Ok(new {message = "success get data", status = true, data = Cust });
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int Id)
        {
            var Cust = _context.Customer.Find(Id);
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpPost]
        public IActionResult CustomerAdd()
        {
            var custi = new Customers
            {
                full_name = "Jonathan Joestar",
                username = "JoJo",
                email = "JJBA@araki.com",
                phone_number = "32676546342",
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Customer.Add(custi);
            _context.SaveChanges();
            return Ok(new { message = "success post data", status = true, data = custi });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int Id)
        {
            var Cust = _context.Customer.First(X=> X.id == Id);
            _context.Customer.Remove(Cust);
            _context.SaveChangesAsync();

            return Ok(new { message = "success removed data", status = true });
        }

        [HttpPut("{id}")]
        public IActionResult PutCustomer([FromBody]JsonPatchDocument<Customers> patch, int Id)
        {

            var Cust = _context.Customer.First(a => a.id == Id);
            Cust.full_name = "Joseph Joestar";
            Cust.updated_at = DateTime.Now;
            _context.SaveChangesAsync();

            patch.ApplyTo(_context.Customer.Find(Id));

            return Ok(new { message = "success put data", status = true, data = Cust } );
        }

    }

    



}
