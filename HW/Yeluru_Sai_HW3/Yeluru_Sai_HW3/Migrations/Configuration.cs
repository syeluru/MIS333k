namespace Yeluru_Sai_HW3.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Yeluru_Sai_HW3.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Yeluru_Sai_HW3.DAL.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Yeluru_Sai_HW3.DAL.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Members.AddOrUpdate(x => x.MemberID,
            new Member() { FirstName = "Olivia", LastName = "Pope", Email = "olivia@example.com", PhoneNumber = "512-555-1234", OKToText = true, McCombsMajors = (Member.Majors)Enum.Parse(typeof(Member.Majors), "MIS", true) });
        }
    }
}
