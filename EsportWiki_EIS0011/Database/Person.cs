using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportWiki_EIS0011.Database
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
    }
}
