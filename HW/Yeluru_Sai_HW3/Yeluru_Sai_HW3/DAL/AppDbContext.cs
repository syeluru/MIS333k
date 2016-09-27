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
        //constructor that invokes the base constructor
        public AppDbContext() : base("MyDBConnection") { }

        //create the db set
        public DbSet<Member> Members { get; set; }


    }
}