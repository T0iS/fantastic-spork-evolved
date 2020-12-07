using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Data;

namespace DataLayer.Database.FunctionalityClasses
{
    public class PersonTable
    
    {
        public static String SQL_INSERT = "INSERT INTO Person (Id, First_Name, Last_Name, Birth_Date, Role, Game_Id, Team_Id) VALUES " +
                "(@id, @fname, @lname, @birth, @role, @game_id, @team_id)";
        public static String SQL_UPDATE = "UPDATE Person SET First_Name=@fname, Last_Name=@lname, Birth_Date=@birth," +
            " Role=@role, Game_Id=@game_id, Team_Id=@team_id WHERE Id=@id";
        public static String SQL_UPDATE_PRESTUP = "UPDATE Person SET Team_Id=@team_id WHERE Id=@id";
        public static String SQL_DELETE_ID = "DELETE FROM Person WHERE Id=@id";
        public static String SQL_SELECT = "SELECT * FROM Person";
        public static String SQL_SELECT_ONE = "SELECT * FROM Person WHERE Id=@id";
        public static String SQL_SELECT_PAR = "SELECT * FROM Person WHERE Last_Name=@lname";
        public static String SQL_SELECT_TEAMMATES = "SELECT * FROM Person WHERE Id!=@id AND Team_Id=@tID";
        public static String SQL_UPDATE_GAME = "UPDATE Person SET GAME_ID=@game WHERE TEAM_ID=@team";
        public static String SQL_SELECT_SEARCH = "SELECT * from Person WHERE First_Name LIKE @attr OR Last_Name LIKE @attr";
        

        private static void PrepareCommand(SqlCommand command, Person p)
        {
            
            
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            command.Parameters["@id"].Value = p.Id;


            command.Parameters.Add(new SqlParameter("@fname", SqlDbType.VarChar));
            command.Parameters["@fname"].Value = p.First_Name;

            
            command.Parameters.Add(new SqlParameter("@lname", SqlDbType.VarChar));
            command.Parameters["@lname"].Value = p.Last_Name;

            
            command.Parameters.Add(new SqlParameter("@birth", SqlDbType.Int));
            command.Parameters["@birth"].Value = p.Birth_Date;


            if (p.Role == null)
            {
                command.Parameters.AddWithValue("@role", " ");
            }
            else
            {
                command.Parameters.AddWithValue("@role", p.Role);
            }
            command.Parameters.AddWithValue("@game_id", p.Game_Id.Id);
            command.Parameters.AddWithValue("@team_id", p.Team_Id.Id);

        }

        private static List<Person> Read(SqlDataReader reader)
        {
            List<Person> People = new List<Person>();

            while (reader.Read())
            {
                int i = -1;
                Person m = new Person();
                m.Id = reader.GetInt32(++i);
                m.First_Name = reader.GetString(++i);
                m.Last_Name = reader.GetString(++i);
                if (!reader.IsDBNull(++i))
                {
                    m.Birth_Date = reader.GetInt32(i);
                }
                if (!reader.IsDBNull(++i))
                {
                    m.Role = reader.GetString(i);
                }
                Game g = new Game();
                g.Id = reader.GetInt32(++i);
                g = GameTable.SelectOne(g.Id);
                m.Game_Id = g;

                Team t = new Team();
                t.Id = reader.GetInt32(++i);
                t = TeamTable.SelectOne(t.Id);
                m.Team_Id = t;

                People.Add(m);
            }
            return People;
        }


        public static int Insert(Person p, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, p);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Update(Person p, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            //db.BeginTransaction();
            Person prv = SelectOne(p.Id);
            if (p.Role != "Coach" && prv.Role == "Coach")
            {
                Team ot = TeamTable.SelectOne(p.Team_Id.Id);
                ot.Person_Id = null;
                TeamTable.Update(ot);
            }


            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, p);
           
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            //db.EndTransaction();
           
            return ret;
        }
        public static int UpdateGame(int tID, int gID, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE_GAME);

            command.Parameters.AddWithValue("@team", gID);
            command.Parameters.AddWithValue("@game", tID);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }   


        public static int Prestup(int pID, int tID, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            Person p = null;
            Team t = null;
            db.BeginTransaction();
            try
            {
                p = SelectOne(pID);
                t = TeamTable.SelectOne(tID);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;

            }
            if(p.Game_Id.Id != t.Game_Id.Id)
            {
                p.Game_Id.Id = t.Game_Id.Id;
                try
                {
                    Update(p, db);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    db.Rollback();
                    return -1;
                }
            }
            if(p.Role == "Coach")
            {
                try
                {
                    Team ot = TeamTable.SelectOne(p.Team_Id.Id);
                    ot.Person_Id = null;
                    TeamTable.Update(ot);

                    ot = TeamTable.SelectOne(tID);
                    ot.Person_Id = p;
                    TeamTable.Update(ot);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    db.Rollback();
                    return -1;
                }
            }

            SqlCommand command = db.CreateCommand(SQL_UPDATE_PRESTUP);
            
            command.Parameters.AddWithValue("@team_id", tID);
            command.Parameters.AddWithValue("@id", pID);
            int ret = db.ExecuteNonQuery(command);

            command.Dispose();
            db.EndTransaction();
            if (pDb == null)
            {
                db.Close();
            }
            return ret;
        }


        public static int Delete(int id, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);

            Person p = PersonTable.SelectOne(id);
            if (p.Role == "Coach")
            {
                Team ot = TeamTable.SelectOne(p.Team_Id.Id);
                ot.Person_Id = null;
                TeamTable.Update(ot);

            }

            command.Parameters.AddWithValue("@id", id);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }


        public static List<Person> Select(DatabaseT pDb = null)
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

            List<Person> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }


        public static Person SelectOne(int pID, DatabaseT pDb = null)
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
            command.Parameters["@id"].Value = pID;

            SqlDataReader reader = db.Select(command);

            Person Users = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static List<Person> SelectByParameter(string attr = null, DatabaseT pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_SEARCH);

            attr = "%" + attr + "%";
            command.Parameters.AddWithValue("@attr", attr);
            SqlDataReader reader = db.Select(command);

            List <Person> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static List<Person> SelectTeammates(int pID, DatabaseT pDb = null)
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

            Person tmp = SelectOne(pID);
            
            
            SqlCommand command = db.CreateCommand(SQL_SELECT_TEAMMATES);
            command.Parameters.AddWithValue("@id", pID);
            command.Parameters.AddWithValue("@tID", tmp.Team_Id.Id);

            SqlDataReader reader = db.Select(command);

            List<Person> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }


    }
}
