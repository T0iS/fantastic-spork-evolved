using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportWiki_EIS0011.Database
{
    class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Person Person_Id { get; set; }
        public Address Address_Id { get; set; }
        public Game Game_Id { get; set; }
        public Organisation Organisation_Id { get; set; }
    }
}
