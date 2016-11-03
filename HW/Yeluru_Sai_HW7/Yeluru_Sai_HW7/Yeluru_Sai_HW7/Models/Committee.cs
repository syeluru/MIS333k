using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Yeluru_Sai_HW7.Models
{
    public class Committee
    {
        // scalar properties
        [Required(ErrorMessage = "Committee ID is required.")]
        [Display(Name = "Committee ID")]
        public Int16 CommitteeID { get; set; }

        [Required(ErrorMessage = "Committee Name is required.")]
        [Display(Name = "Committee Name")]
        public String CommitteeName { get; set; }

        // navigational properties
        public virtual List<Event> Events { get; set; }
        

    }
}