using System.Data.Entity;
using Yeluru_Sai_HW6.Models;

namespace Yeluru_Sai_HW6.DAL
{
    public class AppDbContext : DbContext
    {
        //constructor that invokes the base constructor
        public AppDbContext() : base("MyDBConnection") { }

        //create the db sets
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Frequency> Frequencies { get; set; }

    }
}