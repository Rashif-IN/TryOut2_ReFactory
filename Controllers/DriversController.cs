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
    [Route("driver")]
    public class DriversController : ControllerBase
    {


        private readonly ILogger<DriversController> _logger;

        private readonly OnlineOrderContext _context;

        public DriversController(ILogger<DriversController> logger, OnlineOrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDriver()
        {
            var Cust = _context.Driver;
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpGet("{id}")]
        public IActionResult GetDriverById(int Id)
        {
            var Cust = _context.Driver.Find(Id);
            return Ok(new { message = "success get data", status = true, data = Cust });
        }

        [HttpPost]
        public IActionResult DriverAdd()
        {
            var custi = new Drivers
            {
                full_name = "Speedwagon",
                phone_number = "6533456432",
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Driver.Add(custi);
            _context.SaveChanges();
            return Ok(new { message = "success post data", status = true, data = custi });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDriver(int Id)
        {
            var Cust = _context.Driver.Find(Id);
            _context.Driver.Remove(Cust);
            _context.SaveChangesAsync();

            return Ok(Cust);
        }

      

        [HttpPut("{id}")]
        public IActionResult PutOrder([FromBody]JsonPatchDocument<Drivers> patch, int Id)
        {

            var Cust = _context.Drivers.First(a => a.id == Id);
            Cust.order_status = "not ok";
            Cust.updated_at = DateTime.Now;
            _context.SaveChangesAsync();

            patch.ApplyTo(_context.Drivers.Find(Id));

            return Ok(new { message = "success put data", status = true, data = Cust } );
        }
    }

    

}
