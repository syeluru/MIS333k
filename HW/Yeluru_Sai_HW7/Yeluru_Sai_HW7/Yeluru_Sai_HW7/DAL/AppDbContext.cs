using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yeluru_Sai_HW7.Models;
using System.Data.Entity;

namespace Yeluru_Sai_HW7.DAL
{
    public class AppDbContext : DbContext
    {
        //constructor that invokes the base constructor
        public AppDbContext() : base("MyDBConnection") { }

        //create the db sets
        public DbSet<Member> Members { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Committee> Committees { get; set; }

    }
}