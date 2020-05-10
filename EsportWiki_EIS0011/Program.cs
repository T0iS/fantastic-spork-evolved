using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EsportWiki_EIS0011.Database.FunctionalityClasses;
using EsportWiki_EIS0011.Database;
using System.Collections.ObjectModel;

namespace EsportWiki_EIS0011
{
    static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
             Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new Form1());


            DatabaseT db = new DatabaseT();
            db.Connect();

           
            /*


            Console.WriteLine(" ------------------------------------- 1. Evidence hracu ------------------------------------- ");

            Console.WriteLine("1.a) Zaregistrovani noveho hrace");
            Person p = new Person();
            p.Id = 66;
            p.First_Name = "John";
            p.Last_Name = "Test";
            p.Birth_Date = 2001;
            p.Role = "Player";
           
            p.Game_Id = GameTable.SelectOne(2, db);
            p.Team_Id = TeamTable.SelectOne(3, db);

            Console.WriteLine("Pridano "+ PersonTable.Insert(p, db)+" zaznamu");

            

            Console.WriteLine("1.b) Aktualizace informaci o hraci");
            p.First_Name = "Jack";
            
            Console.WriteLine("Upraveno " + PersonTable.Update(p, db) + " zaznamu");

            /* 
            // Alternativa

            Person toBeChanged = PersonTable.SelectOne(66);
            toBeChanged.First_Name = "Jack";
            PersonTable.Update(toBeChanged, db);

            */
            /*
            Console.WriteLine("1. c) Odstraneni hrace");
            Console.WriteLine("Odstraneno " + PersonTable.Delete(66) +" zaznamu");


            Console.WriteLine("1. d) Seznam hracu");
            Collection <Person> pr = PersonTable.Select();
            int i = 0;
            foreach(Person w in pr)
            {
                Console.WriteLine(w.First_Name + " " + w.Last_Name);
                i++;
                if (i > 5) break;
            }
            Console.WriteLine("Nalezeno " + pr.Count + " zaznamu.");


            Console.WriteLine("1. e) Informace o hraci");
            Person ps = PersonTable.SelectOne(2);
            Console.WriteLine("Hrac: " + ps.First_Name+" "+ps.Last_Name + " nalezen pod ID:" + ps.Id);


            Console.WriteLine("1. f) Seznam spoluhracu");
            pr = PersonTable.SelectTeammates(2);
            Console.WriteLine("Spoluhraci hrace s ID: 2\n");
            foreach (Person w in pr){
                Console.WriteLine(w.First_Name + " " + w.Last_Name);
            }


            Console.WriteLine("1. g) Prestup");
            Console.WriteLine("Pocet zmenenych radku: "+PersonTable.Prestup(7, 1));



            Console.WriteLine("1. h) Seznam hracu s prijmenim Bluett nebo podobnym");
            pr = PersonTable.SelectByParameter("Bluett", db);
            foreach (Person w in pr)
            {
                Console.WriteLine(w.First_Name + " " + w.Last_Name);
            }
            Console.WriteLine("Nalezeno " + pr.Count + " zaznamu.");



            Console.WriteLine(" ------------------------------------- 2. Evidence tymu ------------------------------------- ");

            Console.WriteLine("2. a) Zaregistrovani noveho tymu");
            Team t = new Team();
            t.Id = 7;
            t.Name = "DAIS boys";
            t.Person_Id = null;
            t.Organisation_Id = OrganisationTable.SelectOne(2);
            t.Game_Id = GameTable.SelectOne(2);
            Address a = new Address();
            a.Id = 3;
            t.Address_Id = a;
            Console.WriteLine("Pridano "+ TeamTable.Insert(t)+" radku");


            Console.WriteLine("2. b) Aktualizace informaci o tymu");
            t = TeamTable.SelectOne(7);
            t.Name = "DAIS grlz";
            TeamTable.Update(t);



            Console.WriteLine("2. c) Odstraneni tymu");
            Console.WriteLine("Odebrano " + TeamTable.Delete(7) + " radku");


            Console.WriteLine("2. d) Seznam tymu");
            Collection<Team> tc = TeamTable.Select();
            foreach (Team tt in tc)
            {
                Console.WriteLine(tt.Name);
            }
            Console.WriteLine("Nalezeno " + tc.Count + " zaznamu.");


            Console.WriteLine("2. e) Zmena hry celeho tymu a jeho hracu");
            t = TeamTable.SelectOne(1);
            Console.WriteLine("Zmeneno "+(TeamTable.ZmenitHru(t, 2)+1) +" zaznamu");


            Console.WriteLine("2. f) Vypis organizace, ktera tym spravuje a vlastni");
            Console.WriteLine("ID organizace: "+ TeamTable.SelectOrganisation(2).Id);


            Console.WriteLine("2. g) Prirazeni organizace tymu");
            t = TeamTable.SelectOne(3);
            t.Organisation_Id = OrganisationTable.SelectOne(2);
            Console.WriteLine("Upraveno " + TeamTable.Update(t) + " zaznamu.");


            Console.WriteLine("2. h) Pocet tymu hrajici danou hru");
            Console.WriteLine("Hru hraje: " + TeamTable.SelectCountGame(1) + " tymu");


            Console.WriteLine("2. i) Vypis tymu dle parametru (nazev)");
            tc = TeamTable.SelectByParam("Dignitas", db);
            Console.WriteLine("Nalezeno " + tc.Count + " zaznamu.");




            Console.WriteLine(" ------------------------------------- 3. Evidence Organizaci ------------------------------------- ");

            Console.WriteLine("3. a) Zaregistrovani nove organizace");
            Organisation o = new Organisation();
            o.Id = 77;
            o.Name = "DAIS ORG";
            o.Person_Id = PersonTable.SelectOne(2);
            o.Address_Id = a;
            Console.WriteLine("Pridano " + OrganisationTable.Insert(o, db) + " zaznamu");

            Console.WriteLine("3. b) Aktualizace informaci o organizaci");
            o = OrganisationTable.SelectOne(77);
            o.Name = "Nove jmeno organizace";
            Console.WriteLine("Upraveno " + OrganisationTable.Update(o) + " radku");


            Console.WriteLine("3. c) Zmena kontaktni osoby / manazera");
            o = OrganisationTable.SelectOne(77);
            Person newManager = PersonTable.SelectOne(5);
            Console.WriteLine("Upraveno " + OrganisationTable.UpdateManager(o, newManager.Id, true) + " radku");



            Console.WriteLine("3. d) Vypis vsech osob v tymech spadajicich pod organizaci");
            Collection<Person> co = OrganisationTable.SelectPeople(1);

            foreach (Person pp in co)
            {
                Console.WriteLine(pp.First_Name + " " + pp.Last_Name);
            }
            Console.WriteLine("Nalezeno " + co.Count + " zaznamu.");


           
            Console.WriteLine("3. e) Pocet tymu v organizaci");
            Console.WriteLine("V organizaci je: " + OrganisationTable.SelectTeamCount(2) + " tymu");




            Console.WriteLine("3. d) Odstraneni organizace");
            Console.WriteLine("Odstraneno: " + OrganisationTable.Delete(77) + " zaznamu");







            Console.WriteLine(" ------------------------------------- 4. Evidence zapasu ------------------------------------- ");



            Console.WriteLine("4. a) Pridani noveho zapasu");

            int rowCount = 0;
            Match m = new Match();
            m.Id = 7;
            m.MNUM = 4;
            m.MATCHDATE = "2019-11-09";
            m.Win = "1";
            m.Score = 5;
            m.Game_Id = GameTable.SelectOne(2);
            m.Event_Id = EventTable.SelectOne(1);

            rowCount+=MatchTable.Insert(m, db);

            m.Id = 8;
            m.MNUM = 4;
            m.MATCHDATE = "2019-11-09";
            m.Win = "0";
            m.Score = 2;
            m.Game_Id = GameTable.SelectOne(2);
            m.Event_Id = EventTable.SelectOne(1);

            rowCount += MatchTable.Insert(m, db);

            Console.WriteLine("Zapsano: " + rowCount + " radku");



            Console.WriteLine("4. b) Aktualizace informaci o zapase");
            m.Score = 1;
            Console.WriteLine("Zmeneno: " + MatchTable.Update(m, db) + " zaznamu");



            Console.WriteLine("4. c) Smazani zapasu");
            
            Console.WriteLine("Zmeneno: " + MatchTable.Delete(4) + " zaznamu");



            


            Console.WriteLine(" ------------------------------------- 5. Evidence turnaju/lig ------------------------------------- ");


            Console.WriteLine("5. a) Pridani nove akce");
            Event e = new Event();
            e.Id = 77;
            e.Name = "DAIS league";
            e.Organiser = "VSB";
            e.Prizepool = 200;

            Console.WriteLine("Pridano: " + EventTable.PridatAkci(e, db) + " zaznamu");



            Console.WriteLine("5. b) Vypis akci abecedne");
            Collection<Event> ce = EventTable.Select();

            foreach (Event ee in ce)
            {
                Console.WriteLine(ee.Name);
            }
            Console.WriteLine("Nalezeno " + ce.Count + " zaznamu.");


            Console.WriteLine("5. c) Aktualizace informaci o akci");
            e = EventTable.SelectOne(4);
            e.Name = "DAIS VSB league top";
            Console.WriteLine("Upraveno " + EventTable.Update(e) + " zaznamu.");



            Console.WriteLine("5. d) Smazani zaznamu");            
            Console.WriteLine("Smazano " + EventTable.Delete(4) + " zaznamu.");



            Console.WriteLine(" ------------------------------------- 6. Evidence her ------------------------------------- ");

            Console.WriteLine("6. a) Pridani hry");
            Game g = new Game();
            g.Id = 77;
            g.Name = "DAIS";       
            Console.WriteLine("Pridano " + GameTable.Insert(g, db) + " zaznamu.");


            Console.WriteLine("6. b) Upraveni hry");
            g = GameTable.SelectOne(77);
            g.Name = "DAIS - nove survival RPG";
            Console.WriteLine("Upraveno " + GameTable.Update(g) + " zaznamu.");


            Console.WriteLine("6. c) Vypis vsech her");
            Collection<Game> cg = GameTable.Select();
            foreach (Game gg in cg)
            {
                Console.WriteLine(gg.Name);
            }
            Console.WriteLine("Nalezeno " + cg.Count + " zaznamu.");




            Console.WriteLine("6. d) Odebrani hry");
            Console.WriteLine("Smazano " + GameTable.Delete(77) + " zaznamu.");




            Console.WriteLine(" ------------------------------------- 7. Evidence Uzivatelu ------------------------------------- ");

            Console.WriteLine("7. a) Pridani noveho uzivatele");
            System_user s = new System_user();
            s.Id = 77;
            s.Username = "DAISuser123";
            s.Rights = "User";
            Console.WriteLine("Pridano " + System_userTable.Insert(s, db) + " zaznamu.");



            Console.WriteLine("7. c) Upraveni uzivatelskeho profilu");
            s.Username = "DAISuzivatel321";
            Console.WriteLine("Upraveno " + System_userTable.Update(s) + " zaznamu.");

            Console.WriteLine("7. b) Odebrani uzivatele");
            Console.WriteLine("Smazano " + System_userTable.Delete(77) + " zaznamu.");

            */
        }
    }
}
