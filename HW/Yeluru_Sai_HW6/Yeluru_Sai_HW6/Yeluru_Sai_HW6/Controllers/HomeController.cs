using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yeluru_Sai_HW6.DAL;
using Yeluru_Sai_HW6.Models;

namespace Yeluru_Sai_HW6.Controllers
{
    public enum Gender
    {
        All,
        Male,
        Female
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public enum Operation
    {
        GreaterThan,
        LessThan,
        EqualTo
    }

    public class HomeController : Controller
    {
        public static AppDbContext db = new AppDbContext();
        
        // GET: Home
        public ActionResult Index(string SearchString)
        {
            // create the list of customers with no data
            List<Customer> SelectedCustomers = new List<Customer>();
            ViewBag.NumberAllCustomers = db.Customers.ToList().Count;

            if (SearchString == "" || SearchString == null)
            {
                //set the number of customers into the viewbag for display purposes
                ViewBag.NumberSelectedCustomers = db.Customers.ToList().Count;
                // display the list of customers
                return View(db.Customers.ToList());
            } else {
                // get only the customers that are searched for
                SelectedCustomers = db.Customers.Where(c => c.FirstName.Contains(SearchString) || c.LastName.Contains(SearchString)).ToList();
                //set the number of customers into the viewbag for display purposes
                ViewBag.NumberSelectedCustomers = SelectedCustomers.ToList().Count;
                //sort the list of customers
                SelectedCustomers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.AverageSale);
                // display the list of customers
                return View(SelectedCustomers);
            }

            

        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllFrequencies = GetAllFrequencies();
            return View();
        }

        public ActionResult SearchResults(string SearchString, Int32 SelectedFrequency, Gender SelectedGender, string SalesAmountString, Operation SelectedOperation, SortOrder SelectedSortOrder)
        {
            var query = from c in db.Customers
                        select c;
            // Search the SearchString for names
            if (SearchString == null || SearchString == "")
            {
                query = query;
            } else
            {
                query = query.Where(c => c.FirstName.Contains(SearchString) || c.LastName.Contains(SearchString));
            }

            // filter query by frequency

            if (SelectedFrequency == 0)
            {
                query = query;
            } else
            {
                query = query.Where(c => c.Frequency.FrequencyID == SelectedFrequency);
            }

            // filter query by gender

            if (SelectedGender == Gender.All)
            {
                query = query;
            } else
            {
                query = query.Where(c => c.Gender == SelectedGender.ToString());
            }

            // filter query by sales amount

            if (SalesAmountString == null || SalesAmountString == "")
            {
                query = query;
            } else
            {
                Decimal DecSalesAmountString = Convert.ToDecimal(SalesAmountString);
                if (SelectedOperation == Operation.EqualTo)
                {
                    query = query.Where(c => c.AverageSale == DecSalesAmountString);
                } else if (SelectedOperation == Operation.GreaterThan) {
                    query = query.Where(c => c.AverageSale >= DecSalesAmountString);
                } else
                {
                    query = query.Where(c => c.AverageSale <= DecSalesAmountString);
                }

                
            }

            if (SelectedSortOrder == SortOrder.Descending)
            {
                query = query.OrderByDescending(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.AverageSale);
            } else
            {
                query = query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.AverageSale); ;
            }

            List<Customer> SelectedCustomers = query.ToList();

            // for display purposes:
            ViewBag.NumberAllCustomers = db.Customers.ToList().Count;
            ViewBag.NumberSelectedCustomers = SelectedCustomers.Count;

            return View("Index", SelectedCustomers);
        }

        public SelectList GetAllFrequencies()
        {
            var query = from c in db.Customers
                        orderby c.Frequency.Name
                        select c.Frequency;
            List<Frequency> CustomerList = query.Distinct().ToList();

            //Add in choice for not selecting a frequency
            Frequency NoChoice = new Frequency() { FrequencyID = 0, Name = "All Frequencies" };
            CustomerList.Add(NoChoice);
            SelectList FrequencyList = new SelectList(CustomerList.OrderBy(f => f.Name), "FrequencyID", "Name");
            return FrequencyList;
        }
    }
}