using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Database
{
    public class Match
    {
        public int Id { get; set; }
        public int MNUM { get; set; }
        public string MATCHDATE { get; set; }
        public string Win { get; set; }
        public int Score { get; set; }
        public Game Game_Id { get; set; }
        public Event Event_Id { get; set; }
    }
}
