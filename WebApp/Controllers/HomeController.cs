using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
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
            //ViewBag.Message = "text";
            

        }
        
        public ActionResult Edit(int id)
        {
            Person p = PersonTable.SelectOne(id);

            p.Team_Id = TeamTable.SelectOne(p.Team_Id.Id);

            return View(p);
        }

        [HttpPost]
        public ActionResult tryToEdit(int id, string firstname, string lastname, string role, string team, int bdate, string game)
        {
            Team t = TeamTable.SelectOneByParam(team);
            Game g = GameTable.SelectOneParam(game);
            Person p = new Person(id, firstname, lastname, role, g, t, bdate);
            PersonTable.Update(p);

            return RedirectToAction("Players");
        }

        [HttpPost]
        public ActionResult add(int id, string firstname, string lastname, string role, string team, int bdate, string game)
        {

        }

    }
}