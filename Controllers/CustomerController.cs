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

            return Ok(new { message = "success removed data", status = true, data = Cust } );
        }

    }

    [Command(Description = "Show items in list", Name = "list")]
    class List_
    {
        public async Task OnExecuteAsync()
        {
            var Act = new List<string>();
            var client = new HttpClient();
            var result = await client.GetStringAsync("http://localhost:5000/api/v1/customer");
            var Jsonn = JsonConvert.DeserializeObject<List<Customers>>(result);
            foreach (var X in Jsonn)
            {

                Act.Add(
                    $"{X.full_name} \n " +
                    $"{X.username} \n" +
                    $"{X.email} \n" +
                    $"{X.phone_number} \n" +
                    $"{X.created_at} \n" +
                    $"{X.updated_at} \n");
            }
            Console.WriteLine(String.Join("\n \n", Act));

        }
    }

    [Command(Description = "Add items in list", Name = "add")]
    class Add_
    {
        //[Argument(0)]
        //public int id { get; set; }
        [Argument(0)]
        public string Full_name { get; set; }
        [Argument(1)]
        public string Username { get; set; }
        [Argument(2)]
        public string Email { get; set; }
        [Argument(3)]
        public string Phone_number { get; set; }

        public async Task OnExecuteAsync()
        {
            var client = new HttpClient();
            var AddText = new Customers()
            {
                full_name = Full_name,
                username = Username,
                email = Email,
                phone_number = Phone_number,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            var data = new StringContent(JsonConvert.SerializeObject(AddText), Encoding.UTF8, "application/json");
            await client.PostAsync("http://localhost:5000/api/v1/customer", data);
        }
    }

    //[Command(Description = "Update item in list", Name = "update")]
    //class Update_
    //{
    //    [Argument(0)]
    //    public string ID { get; set; }
    //    [Argument(1)]
    //    public string text { get; set; }

    //    public async Task OnExecuteAsync()
    //    {
    //        var client = new HttpClient();
    //        var request = new { id = Convert.ToInt32(ID), activity = text };
    //        var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
    //        await client.PatchAsync($"http://localhost:5000/api/v1/customer/{ID}", data);
    //    }
    //}

    [Command(Description = "Delete item in list", Name = "delete")]
    class Delete_
    {
        [Argument(0)]
        public string ID { get; set; }

        public async Task OnExecuteAsync()
        {
            var client = new HttpClient();
            var request = new { id = Convert.ToInt32(ID) };
            await client.DeleteAsync($"http://localhost:5000/api/v1/customer/{ID}");
        }
    }



}
