using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using BusinessLayer;

namespace DataLayer.Database.FunctionalityClasses
{
    public class GameTable
    {
        public static string SQL_INSERT = "INSERT INTO Game (Id, Name) VALUES (@id, @name)";
        public static String SQL_UPDATE = "UPDATE Game SET Name=@name WHERE Id=@id";
        public static String SQL_DELETE_ID = "DELETE FROM Game WHERE Id=@id";
        public static String SQL_SELECT = "SELECT * FROM Game";
        public static String SQL_SELECT_ONE = "SELECT * FROM Game WHERE Id=@id";
        public static String SQL_SELECT_ONE_PARAM = "SELECT * FROM Game WHERE Name=@name";

        private static void PrepareCommand(SqlCommand command, Game game)
        {

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            command.Parameters["@id"].Value = game.Id;

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar));
            command.Parameters["@name"].Value = game.Name;

        }

        private static Collection<Game> Read(SqlDataReader reader)
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
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, game);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Update(Game game, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);
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

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Game> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static Game SelectOneParam(string name, DatabaseT pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_ONE_PARAM);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, name.Length));
            command.Parameters["@name"].Value = name;
            SqlDataReader reader = db.Select(command);

            Game g = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return g;
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_ONE);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            command.Parameters["@id"].Value = gID;
            SqlDataReader reader = db.Select(command);

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
            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue("@id", id);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }



    }
}
