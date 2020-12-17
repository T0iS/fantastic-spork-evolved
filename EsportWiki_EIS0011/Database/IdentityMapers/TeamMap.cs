using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataLayer.Database.FunctionalityClasses;

namespace DataLayer.Database.IdentityMapers
{
    public class TeamMap
    {

        private static Dictionary<int, Team> teamMap = new Dictionary<int, Team>();
        private static bool gotAll = false;


        public static void addTeam(Team g)
        {
            teamMap.Add(g.Id, g);
        }

        public static void addTeamList(List<Team> Teams)
        {
            foreach (Team g in Teams)
            {
                addTeam(g);
            }
        }

        public static Team getTeam(int id)
        {
            if (!teamMap.ContainsKey(id))
                addTeam(TeamTable.SelectOne(id));
            return teamMap[id];
        }

        public static Team getTeamByName(string name)
        {
            if (gotAll == false)
            {
                getAll();
            }
                foreach (KeyValuePair<int, Team> t in teamMap)
            {
                if(t.Value.Name == name)
                {
                    return t.Value;
                }
            }
            return null;
        }


        public static List<Team> getAll()
        {
            if (gotAll == false)
            {
                deleteData();
                List<Team> l = TeamTable.Select();
                addTeamList(l);
                gotAll = true;
                return teamMap.Values.ToList();
            }
            return teamMap.Values.ToList();
        }

        public static void setGotAllFalse()
        {
            gotAll = false;
        }

        public static void deleteData()
        {
            teamMap.Clear();
        }

    }
}
