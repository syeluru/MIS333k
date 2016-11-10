namespace IdentityTemplate.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using IdentityTemplate.Models;
    using Microsoft.AspNet.Identity;
    using System.Web;
    internal sealed class Configuration : DbMigrationsConfiguration<IdentityTemplate.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IdentityTemplate.Models.AppDbContext context)
        {
            //Update seed method to have one admin
            AppDbContext db = new AppDbContext();

          


        }
    }
}
