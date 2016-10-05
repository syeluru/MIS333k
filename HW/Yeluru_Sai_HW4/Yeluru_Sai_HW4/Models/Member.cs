using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Yeluru_Sai_HW4.Models
{
    public class Member
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

        // scalar properties

        [Required(ErrorMessage = "Member ID is required.")]
        [Display(Name = "Customer ID")]
        public Int16 MemberID { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Email Address")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please ensure whether it is ok to text or not.")]
        [Display(Name = "Ok To Text?")]
        public Boolean OKToText { get; set; }

        [Required(ErrorMessage = "Major is required.")]
        [Display(Name = "Major?")]
        [EnumDataType(typeof(Majors))]
        public Majors McCombsMajors { get; set; }

        // navigational properties
        public virtual List<Event> Events { get; set; }

    }
}