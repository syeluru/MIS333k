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
    public class EventsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            ViewBag.SponsoringCommittee = @event.SponsoringCommittee;
            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.AllCommittees = GetAllCommittees();
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "EventID,EventTitle,EventDate,EventLocation,OKToText")] Event @event, Int32 CommitteeID)
        {
            //find selected committee
            Committee SelectedCommittee = db.Committees.Find(CommitteeID);

            //associate committee with event
            @event.SponsoringCommittee = SelectedCommittee;


            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AllCommittees = GetAllCommittees(@event);
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            //Add to viewbag
            ViewBag.AllCommittees = GetAllCommittees(@event);
            ViewBag.AllUsers = GetAllUsers(@event);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "EventID,EventTitle,EventDate,EventLocation,MembersOnly")] Event @event, Int32 CommitteeID, string[] SelectedUsers)
        {
            if (ModelState.IsValid)
            {
                //Find associated event
                Event eventToChange = db.Events.Find(@event.EventID);

                //change committee if necessary
                if (eventToChange.SponsoringCommittee.CommitteeID != CommitteeID)
                {
                    //find committee
                    Committee SelectedCommittee = db.Committees.Find(CommitteeID);

                    //update committee
                    eventToChange.SponsoringCommittee = SelectedCommittee;
                }

                //remove any existing members
                eventToChange.AppUsers.Clear();

                //if there are members to add, add them
                if (SelectedUsers != null)
                {
                    foreach (string Id in SelectedUsers)
                    {
                        AppUser userToAdd = db.Users.Find(Id);
                        eventToChange.AppUsers.Add(userToAdd);
                    }
                }

                //update the rest of the fields
                eventToChange.EventTitle = @event.EventTitle;
                eventToChange.EventDate = @event.EventDate;
                eventToChange.EventLocation = @event.EventLocation;
                eventToChange.MembersOnly = @event.MembersOnly;

                //update the db.Entry code to reflect the event you want to change and save changes
                db.Entry(eventToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            //re-populate lists
            //Add to viewbag
            ViewBag.AllCommittees = GetAllCommittees(@event);
            ViewBag.AllUsers = GetAllUsers(@event);

            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize (Roles = "Admin")]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public SelectList GetAllCommittees(Event @event) //COMMITTEE ALREADY CHOSEN
        {
            //populate list of committees
            var query = from c in db.Committees
                        orderby c.CommitteeName
                        select c;
            //create list and execute query
            List<Committee> allCommittees = query.ToList();

            //convert to select list
            SelectList list = new SelectList(allCommittees, "CommitteeID", "CommitteeName", @event.SponsoringCommittee.CommitteeID);
            return list;
        }

        public SelectList GetAllCommittees() //NO COMMITTEE CHOSEN
        {
            //create query to find all committees
            var query = from c in db.Committees
                        orderby c.CommitteeName
                        select c;
            //create list and execute query
            List<Committee> allCommittees = query.ToList();

            //convert to select list
            SelectList allCommitteesList = new SelectList(allCommittees, "CommitteeID", "CommitteeName");
            return allCommitteesList;
        }

        public MultiSelectList GetAllUsers(Event @event)
        {
            //find the list of members
            var query = from m in db.Users
                        orderby m.Email
                        select m;


            //convert to list and execute query
            List<AppUser> allUsers = query.ToList();

            //create list of selected members
            List<string> SelectedUsers = new List<string>();

            //Loop through list of members and add MemberId
            foreach (AppUser m in @event.AppUsers)
            {
                SelectedUsers.Add(m.Id);
            }

            //convert to multiselect
            MultiSelectList allUsersList = new MultiSelectList(allUsers, "Id", "Email", SelectedUsers);

            // this line is important when they do it again
            //ViewBag.AllMembers = GetAllMembers(@event);
            return allUsersList;
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
