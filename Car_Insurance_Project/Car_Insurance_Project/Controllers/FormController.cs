using System;
using Car_Insurance_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Car_Insurance_Project.Controllers
{
    public class FormController : Controller
    {
        // GET: Form
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Policy()
        {
            var totalPrice = 50;
            var applicant = new Application();
            if (applicant.Age < 18)
            {
                totalPrice += 100;
            }
            else if (applicant.Age < 25)
            {
                totalPrice += 25;
            }
            else if (applicant.Age > 100)
            {
                totalPrice += 25;
            }
            else if (applicant.Car_Year < 2000)
            {
                totalPrice += 25;
            }
            else if (applicant.Car_Year > 2015)
            {
                totalPrice += 25;
            }
            else if (applicant.Make == "Porsche")
            {
                totalPrice += 25;
            }
            else if (applicant.Make == "Porsche" && applicant.Model == "911 Carrera")
            {
                totalPrice += 50;
            }
            return View();
        }
    }
}