using Car_Insurance_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Car_Insurance_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string FirstName, string LastName, int Age, 
                                  string EmailAddress, int Car_Year, string Make,
                                   string Model, bool Dui, int Tickets,
                                   bool Full_Coverage, bool Liability)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(Make) || string.IsNullOrEmpty(Model))
            {
                return View("~/Views/Shared/Error.cshtml");
            }

            else
            {
                using (Car_InsuranceEntities db = new Car_InsuranceEntities())
                    {
                    var applicant = new Application
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Age = Age,
                        EmailAddress = EmailAddress,
                        Car_Year = Car_Year,
                        Make = Make,
                        Model = Model,
                        Dui = Dui,
                        Tickets = Tickets,
                        Full_Coverage = Full_Coverage,
                        Liability = Liability
                    };

                    db.Applications.Add(applicant);
                        db.SaveChanges();
                    }
            }
            return View("Contact");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}