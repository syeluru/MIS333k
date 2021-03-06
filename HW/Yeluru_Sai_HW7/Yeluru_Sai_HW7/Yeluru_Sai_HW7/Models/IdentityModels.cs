﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

//TODO: Change the namespace here to match your project's name
namespace Yeluru_Sai_HW7.Models
{

    public enum Majors
    {
        Accounting,
        [Display(Name = "Business Honors")]
        BusinessHonors,
        Finance,
        [Display(Name = "International Business")]
        InternationalBusiness,
        Management,
        [Display(Name = "Management Information Systems")]
        MIS,
        Marketing,
        [Display(Name = "Supply Chain Management")]
        SupplyChainManagement,
        [Display(Name = "Science and Technology Management")]
        STM
    };

    // You can add profile data for the user by adding more properties to your AppUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class AppUser : IdentityUser
    {
        //TODO: Put any additional fields that you need for your users here
        //For example:

        //public Int32 UserID { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        //public String Email { get; set; }

        //public String PhoneNumber { get; set; }

        public Boolean OKToText { get; set; }

        public Majors McCombsMajors { get; set; }

        // navigational properties
        public virtual List<Event> Events { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    //NOTE: Here is your dbContext for the entire project.  There should only be ONE DbContext per project
    //Your dbContext (AppDbContext) inherits from IdentityDbContext, which inherits from the "regular" DbContext
    //TODO: If you have an existing dbContext (it may be in your DAL folder), DELETE THE EXISTING dbContext

    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //TODO: Add your dbSets here.  As an example, I've included one for products
        //Remember - the IdentityDbContext already contains a db set for Users.  If you add another one, your code will break
        public DbSet<Committee> Committees { get; set; }
        public DbSet<Event> Events { get; set; }
        //public DbSet<AppUser> Users { get; set; }

                
        public AppDbContext()
            : base("MyDbConnection", throwIfV1Schema: false)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
        
        //Add dbSet for roles
         public DbSet<AppRole> AppRoles { get; set; }
    }
}
