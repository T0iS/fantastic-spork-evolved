using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Person
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int? Birth_Date { get; set; }
        public string Role { get; set; }
        public Game Game_Id { get; set; } 
        public Team Team_Id { get; set; }

        public Person()
        {
        }

        public Person(int id, string fname, string lname, string role, Game g, Team t, int bdate = 0)
        {
            this.Id = id;
            this.First_Name = fname;
            this.Last_Name = lname;
            this.Role = role;
            this.Game_Id = g;
            this.Team_Id = t;
            this.Birth_Date = bdate;


        }


    }

    
}
