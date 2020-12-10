using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataLayer.Database.FunctionalityClasses;

namespace DataLayer.Database.IdentityMapers
{
    public class GameMap
    {

        private static Dictionary<int, Game> gameMap = new Dictionary<int, Game>();
        private static bool gotAll = false;


        public static void addGame(Game g)
        {
            gameMap.Add(g.Id, g);
        }

        public static void addGameList(List<Game> games)
        {
            foreach (Game g in games)
            {
                addGame(g);
            }
        }

        public static Game getPerson(int id)
        {
            if (!gameMap.ContainsKey(id))
                addGame(GameTable.SelectOne(id));
            return gameMap[id];
        }


        public static List<Game> getAll()
        {
            if (gotAll == false)
            {
                deleteData();
                List<Game> l = GameTable.Select();
                addGameList(l);
                gotAll = true;
                return gameMap.Values.ToList();
            }
            return gameMap.Values.ToList();
        }

        public static void setGotAllFalse()
        {
            gotAll = false;
        }

        public static void deleteData()
        {
            gameMap.Clear();
        }
    }
}
