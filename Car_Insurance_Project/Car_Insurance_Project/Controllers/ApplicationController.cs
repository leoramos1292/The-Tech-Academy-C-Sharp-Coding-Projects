using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Car_Insurance_Project.Models;

namespace Car_Insurance_Project.Controllers
{
    public class ApplicationController : Controller
    {
        private Car_InsuranceEntities db = new Car_InsuranceEntities();

        //This is the main view where all data form the database table is visible
        public ActionResult Index()
        {
            return View(db.Applications.ToList());
        }

        //This allows you to pick one row from the database and edit view the values seperately from the rest
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        //This method allows an Applicant to apply (crerates a new database object)
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Age,EmailAddress,Car_Year,Make,Model,Dui,Tickets,Coverage,Quote")] Application application, int? id)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Confirmation");
            }
            return View(application);
        }

        //This method allows you to delete entire rows from the database
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //This method sets up a view with specific information from the Database
        public ActionResult Admin()
        {
            return View(db.Applications.ToList());
        }

        //This method takes the latest entry in the database and allows you to generate a quote. 
        //It's there for the applicant to confirm the information they entered and allows them to edit it if necessary. 
        public ActionResult Confirmation()
        {
            return View(db.Applications.ToList());
        }

        //This allows the applicant to edit the values they entered before confirming them
        public ActionResult ConfirmEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmEdit([Bind(Include = "Id,FirstName,LastName,Age,EmailAddress,Car_Year,Make,Model,Dui,Tickets,Coverage,Quote")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Confirmation");
            }
            return View(application);
        }

        //this method generates a quote for the applicant depending on what answers were given
        [HttpGet]
        public ActionResult Quote(int? id)
        {
            using (Car_InsuranceEntities db = new Car_InsuranceEntities())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                decimal totalPrice = 50;
                Application application = db.Applications.Find(id);
                //age
                if (application.Age < 18)
                {
                    totalPrice += 100;
                }
                if (application.Age < 25 && application.Age > 17)
                {
                    totalPrice += 25;
                }
                if (application.Age > 100)
                {
                    totalPrice += 25;
                }
                //age end

                //year
                if (application.Car_Year < 2000 || application.Car_Year > 2015)
                {
                    totalPrice += 25;
                }
                //end year

                //make/model
                if (application.Make == "Porsche")
                {
                    totalPrice += 25;
                }
                if (application.Model == "911 Carrera")
                {
                    totalPrice += 25;
                }
                //end make/model

                //violations and percentages
                decimal tickets = application.Tickets * 10;
                totalPrice += tickets;

                if (application.Dui == "Yes" && application.Coverage == "Full Coverage")
                {
                    decimal percentages = totalPrice * .75m;
                    totalPrice += percentages;
                }

                if (application.Dui == "Yes" && application.Coverage == "Liability")
                {
                    decimal percentages = totalPrice * .25m;
                    totalPrice += percentages;
                }

                if (application.Dui == "No" && application.Coverage == "Full Coverage")
                {
                    decimal percentages = totalPrice * .50m;
                    totalPrice += percentages;
                }

                application.Quote = totalPrice;
                db.SaveChanges();
                return View(application);
            }
        }

    }
}
