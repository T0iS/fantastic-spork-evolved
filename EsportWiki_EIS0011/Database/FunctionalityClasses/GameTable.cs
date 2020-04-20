using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;

namespace EsportWiki_EIS0011.Database.FunctionalityClasses
{
    class GameTable
    {
        public static string SQL_INSERT = "INSERT INTO Game (Id, Name) VALUES (:id, :name)";
        public static String SQL_UPDATE = "UPDATE Game SET Name=:name WHERE Id=:id";
        public static String SQL_DELETE_ID = "DELETE FROM Game WHERE Id=:id";
        public static String SQL_SELECT = "SELECT * FROM Game";
        public static String SQL_SELECT_ONE = "SELECT * FROM Game WHERE Id=:id";

        private static void PrepareCommand(OracleCommand command, Game game)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":id", game.Id);
            command.Parameters.AddWithValue(":name", game.Name);
        }

        private static Collection<Game> Read(OracleDataReader reader)
        {
            Collection<Game> Games = new Collection<Game>();

            while (reader.Read())
            {
                int i = -1;
                Game g = new Game();
                g.Id = reader.GetInt32(++i);
                g.Name = reader.GetString(++i);
               
                Games.Add(g);
            }
            return Games;
        }

        public static int Insert(Game game, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, game);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Update(Game game, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, game);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        public static Collection<Game> Select(DatabaseT pDb = null)
        {
            DatabaseT db;
            if (pDb == null)
            {
                db = new DatabaseT();
                db.Connect();
            }
            else
            {
                db = (DatabaseT)pDb;
            }

            OracleCommand command = db.CreateCommand(SQL_SELECT);
            OracleDataReader reader = db.Select(command);

            Collection<Game> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static Game SelectOne(int gID, DatabaseT pDb = null)
        {
            DatabaseT db;
            if (pDb == null)
            {
                db = new DatabaseT();
                db.Connect();
            }
            else
            {
                db = (DatabaseT)pDb;
            }

            OracleCommand command = db.CreateCommand(SQL_SELECT_ONE);
            command.Parameters.AddWithValue(":id", gID);
            OracleDataReader reader = db.Select(command);

            Game g = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return g;
        }

        public static int Delete(int id, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue(":id", id);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }



    }
}
