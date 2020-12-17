using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using DataLayer.Database;
using DataLayer.Database.FunctionalityClasses;
using DataLayer.Database.IdentityMapers;

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

            teams = TeamMap.getAll();
            ViewData["personList"] = teams;

            return View();
        }

        public ActionResult Players()
        {
            

            List<Person> players;

            players = PersonMap.getAll();
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

            p.Team_Id = TeamMap.getTeam(p.Team_Id.Id);

            return View(p);
        }
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult ChangeGame(string id)
        {
            List<Game> games;

            games = GameMap.getAll();
            ViewData["gameList"] = games;
            ViewBag.Message = id;

            return View();
        }

        [HttpPost]
        public ActionResult tryToEdit(string id, string firstname, string lastname, string role, string team, string bdate, string game)
        {
            try
            {
                Team t = TeamMap.getTeamByName(team);
                Game g = GameMap.getGameByName(game);
                Person p = new Person(Int32.Parse(id), firstname, lastname, role, g, t, Int32.Parse(bdate));
                PersonTable.Update(p);
            }
            catch(Exception e)
            {
                
            }
            return RedirectToAction("Players");
        }

        [HttpPost]
        public ActionResult tryToAdd(string firstname, string lastname, string role, string team, string bdate, string game)
        {
            try
            {
                Team t = TeamMap.getTeamByName(team);
                Game g = GameMap.getGameByName(game);
                int id = PersonMap.getAll().Count + 1;
                Person p = new Person(id, firstname, lastname, role, g, t, Int32.Parse(bdate));
                PersonTable.Insert(p);
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Players");
        }

        [HttpGet]
        public ActionResult tryToChangeGame(string id_team, string id_game)
        {
            try { 
                Team t = TeamMap.getTeam(Int32.Parse(id_team));
                TeamTable.ZmenitHru(t, Int32.Parse(id_game));
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Teams");
        }

    }
}