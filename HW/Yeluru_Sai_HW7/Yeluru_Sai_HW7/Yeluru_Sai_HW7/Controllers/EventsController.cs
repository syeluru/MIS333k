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
            ViewBag.AllMembers = GetAllMembers(@event);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,EventTitle,EventDate,EventLocation,MembersOnly")] Event @event, Int32 CommitteeID, int[] SelectedMembers)
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
                eventToChange.Members.Clear();

                //if there are members to add, add them
                if (SelectedMembers != null)
                {
                    foreach (int memberID in SelectedMembers)
                    {
                        Member memberToAdd = db.Members.Find(memberID);
                        eventToChange.Members.Add(memberToAdd);
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
            ViewBag.AllMembers = GetAllMembers(@event);

            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return View(@event);
        }

        // GET: Events/Delete/5
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
        public ActionResult DeleteConfirmed(short id)
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

        public MultiSelectList GetAllMembers(Event @event)
        {
            //find the list of members
            var query = from m in db.Members
                        orderby m.Email
                        select m;


            //convert to list and execute query
            List<Member> allMembers = query.ToList();

            //create list of selected members
            List<Int32> SelectedMembers = new List<Int32>();

            //Loop through list of members and add MemberId
            foreach (Member m in @event.Members)
            {
                SelectedMembers.Add(m.MemberID);
            }

            //convert to multiselect
            MultiSelectList allMembersList = new MultiSelectList(allMembers, "MemberID", "Email", SelectedMembers);

            // this line is important when they do it again
            //ViewBag.AllMembers = GetAllMembers(@event);
            return allMembersList;
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
