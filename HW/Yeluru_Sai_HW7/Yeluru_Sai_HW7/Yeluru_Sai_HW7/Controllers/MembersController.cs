using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Yeluru_Sai_HW7.DAL;
using Yeluru_Sai_HW7.Models;

namespace Yeluru_Sai_HW7.Controllers
{
    public class MembersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,FirstName,LastName,Email,PhoneNumber,OKToText,McCombsMajors")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllEvents = GetAllEvents(member);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,FirstName,LastName,Email,PhoneNumber,OKToText,McCombsMajors")] Member @member, int[] SelectedEvents)
        {
            if (ModelState.IsValid)
            {
                // Find associated member
                Member memberToChange = db.Members.Find(@member.MemberID);

                // change events
                memberToChange.Events.Clear();

                // if there are events to add, add them
                if (SelectedEvents != null)
                {
                    foreach (int selectedEventID in SelectedEvents)
                    {
                        Event eventToAdd = db.Events.Find(selectedEventID);
                        memberToChange.Events.Add(eventToAdd);
                    }
                }

                memberToChange.Email = @member.Email;
                memberToChange.FirstName = @member.FirstName;
                memberToChange.LastName = @member.LastName;
                memberToChange.PhoneNumber = @member.PhoneNumber;
                memberToChange.OKToText = @member.OKToText;
                memberToChange.McCombsMajors = @member.McCombsMajors;

                ViewBag.AllEvents = GetAllEvents(memberToChange);

                //update the db.Entry code to reflect the event you want to change and save changes
                db.Entry(memberToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //repopulate lists
            //Add to viewbag
            ViewBag.AllEvents = GetAllEvents(@member);
            

            return View(@member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
            

        public MultiSelectList GetAllEvents(Member @member)
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
            foreach (Event e in @member.Events)
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
