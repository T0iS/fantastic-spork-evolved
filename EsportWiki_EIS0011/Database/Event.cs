using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportWiki_EIS0011.Database
{
    class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Organiser { get; set; }
        public int? Prizepool { get; set; }
    }
}
