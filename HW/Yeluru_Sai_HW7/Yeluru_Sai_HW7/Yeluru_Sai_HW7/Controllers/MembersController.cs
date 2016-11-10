using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Yeluru_Sai_HW7.Models;

namespace Yeluru_Sai_HW7.Controllers
{
    public class MembersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Members
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Members/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //// GET: Members/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Members/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,OKToText,McCombsMajors")] AppUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

            
        //    return View(user);
        //}

        // GET: Members/Edit/5
        [Authorize(Roles = "Admin, Member")]
        public ActionResult Edit(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // add an if statement to trigger a manual exception if user is trying to change someone else's character
            if (Id != User.Identity.GetUserId<string>() && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }



            AppUser user = db.Users.Find(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllEvents = GetAllEvents(user);
            return View(user);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Member")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,OKToText,McCombsMajors")] AppUser user, int[] SelectedEvents)
        {
            if (ModelState.IsValid)
            {

                // add an if statement to trigger a manual exception if user is trying to change someone else's character
                if (user.Id != User.Identity.GetUserId<string>() && !User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                // Find associated member
                AppUser userToChange = db.Users.Find(user.Id);

                // change events
                userToChange.Events.Clear();



                // if there are events to add, add them
                if (SelectedEvents != null)
                {
                    foreach (int selectedEventID in SelectedEvents)
                    {
                        Event eventToAdd = db.Events.Find(selectedEventID);
                        userToChange.Events.Add(eventToAdd);
                    }
                }

                userToChange.Email = user.Email;
                userToChange.FirstName = user.FirstName;
                userToChange.LastName = user.LastName;
                userToChange.PhoneNumber = user.PhoneNumber;
                userToChange.OKToText = user.OKToText;
                userToChange.McCombsMajors = user.McCombsMajors;

                ViewBag.AllEvents = GetAllEvents(userToChange);

                //update the db.Entry code to reflect the event you want to change and save changes
                db.Entry(userToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //repopulate lists
            //Add to viewbag
            ViewBag.AllEvents = GetAllEvents(user);
            

            return View(user);
        }

        // GET: Members/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            AppUser user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
            

        public MultiSelectList GetAllEvents(AppUser user)
        {
            //find the list of events
            var query = from e in db.Events
                        orderby e.EventTitle
                        select e;

            //convert to list and execute query
            List<Event> allEvents = query.ToList();

            //create list of selected events
            List<Int32> SelectedEvents = new List<Int32>();

            //Loop through list of events and add EventID
            foreach (Event e in user.Events)
            {
                SelectedEvents.Add(e.EventID);
            }

            //convert to multiselectlist
            MultiSelectList allEventsList = new MultiSelectList(allEvents, "EventID", "EventTitle", SelectedEvents);

            return allEventsList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
