using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yeluru_Sai_HW6.Models
{
    public class Frequency
    {
        public Int32 FrequencyID { get; set; }

        public String Name { get; set; }

        //Navigational property for customers
        public virtual List<Customer> Customers { get; set; }
    }
}