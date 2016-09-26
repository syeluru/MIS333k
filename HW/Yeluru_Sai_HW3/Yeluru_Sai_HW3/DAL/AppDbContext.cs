using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yeluru_Sai_HW3.Models;
using System.Data.Entity;

namespace Yeluru_Sai_HW3.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("MyDBConnection") { }

        public DbSet<Customer> Customers { get; set; }


    }
}