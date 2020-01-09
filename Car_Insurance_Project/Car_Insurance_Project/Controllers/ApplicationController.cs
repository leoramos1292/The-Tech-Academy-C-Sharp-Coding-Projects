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

        // GET: Application
        public ActionResult Index()
        {
            return View(db.Applications.ToList());
        }

        // GET: Application/Details/5
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

        // GET: Application/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Application/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Age,EmailAddress,Car_Year,Make,Model,Dui,Tickets,Coverage,Quote")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(application);
        }

        // GET: Application/Edit/5
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

        // POST: Application/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Age,EmailAddress,Car_Year,Make,Model,Dui,Tickets,Coverage,Quote")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }

        // GET: Application/Delete/5
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

        // POST: Application/Delete/5
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

        [HttpGet]
        public ActionResult Quote(int? id)
        {
            using (Car_InsuranceEntities db = new Car_InsuranceEntities())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var totalPrice = 50;
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

                //violations
                var tickets = application.Tickets * 10;
                totalPrice += tickets;

                if (application.Dui == "Yes" && application.Coverage == "Full Cover")
                {
                    var percentages = totalPrice / .75 * 100;
                    Convert.ToDouble(totalPrice);
                }

                application.Quote = totalPrice;
                db.SaveChanges();
                return View(application);
            }
        }

    }
}
