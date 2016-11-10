using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Yeluru_Sai_HW7.Models
{
    public class Event
    {

        //scalar properties
        [Required(ErrorMessage = "Event ID is required.")]
        [Display(Name = "Event ID")]
        public Int16 EventID { get; set; }

        [Required(ErrorMessage = "Event Title is required.")]
        [Display(Name = "Event Title")]
        public String EventTitle { get; set; }

        [Required(ErrorMessage = "Event Date is required.")]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Event Location is required.")]
        [Display(Name = "Location")]
        public String EventLocation { get; set; }

        [Required(ErrorMessage = "Please ensure whether it is members only or open to non-members.")]
        [Display(Name = "Members Only?")]
        public Boolean MembersOnly { get; set; }

        // navigational properties
        public virtual Committee SponsoringCommittee { get; set; }

        public virtual List<AppUser> AppUsers { get; set; }

    }
}