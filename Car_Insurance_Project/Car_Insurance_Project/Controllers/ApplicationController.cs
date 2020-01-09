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

        public ActionResult Admin()
        {
            return View(db.Applications.ToList());
        }

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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Age,EmailAddress,Car_Year,Make,Model,Dui,Tickets,Coverage,Quote")] Application application, int? id)
        {
            using (Car_InsuranceEntities db = new Car_InsuranceEntities())
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                db.Applications.Find(id);
                return RedirectToAction("Admin");
            }

            return View(application);
        }

        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Age,EmailAddress,Car_Year,Make,Model,Dui,Tickets,Coverage,Quote")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(application);
        }

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
            return RedirectToAction("Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

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
                if (application.Age < 25 && application.Age > 18)
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
