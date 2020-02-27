using System;
using System.Collections.Generic;

namespace Tryout2.Controllers
{
    public class Customers
    {
        public int id { get; set; }
        public string full_name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class Orders
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string order_status { get; set; }
        public int driver_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class Order_items
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
    }
    public class Drivers
    {
        public int id { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class Products
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
