using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Database;
using DataLayer.Database.FunctionalityClasses;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Index(string desc, string text)
        {
            Inquiry inq = new Inquiry(desc, text);
            InquiryTable.Insert(inq);
            return View();
        }

        public ActionResult Teams()
        {
           
            List<Team> teams;

            teams = TeamTable.Select();
            ViewData["personList"] = teams;

            return View();
        }

        public ActionResult Players()
        {
            

            List<Person> players;

            players = PersonTable.Select();
            ViewData["personList"] = players;

            return View();
        }

        public void AddNewInquiry()
        {
            //ViewBag.Message = "HUE";
            

        }
    }
}