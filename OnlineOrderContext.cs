using System;
using Microsoft.EntityFrameworkCore;
using Tryout2.Controllers;

namespace Tryout2
{
    public class OnlineOrderContext : DbContext
    {
        public OnlineOrderContext(DbContextOptions<OnlineOrderContext> options): base (options){}
        public DbSet<Customers> Customer { get; set; }
        public DbSet<Orders> Order { get; set; }
        public DbSet<Order_items> Order_Item { get; set; }
        public DbSet<Drivers> Driver { get; set; }
        public DbSet<Products> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=ec2-18-213-176-229.compute-1.amazonaws.com; Username=vddgdmlmbokmja; Password=953104f2bad30f0e98db02816b2f5f5c9e06770acc5c5d8e8519062fc13bba50; Database=dam3oqh3b9pek6; SSL Mode=Require; Trust Server Certificate=true");
    }
}
