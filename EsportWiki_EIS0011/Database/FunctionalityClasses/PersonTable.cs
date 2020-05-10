using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;

namespace EsportWiki_EIS0011.Database.FunctionalityClasses
{
    class PersonTable
    
    {
        public static String SQL_INSERT = "INSERT INTO Person (Id, First_Name, Last_Name, Birth_Date, Role, Game_Id, Team_Id) VALUES " +
                "(:id, :fname, :lname, :birth, :role, :game_id, :team_id)";
        public static String SQL_UPDATE = "UPDATE Person SET First_Name=:fname, Last_Name=:lname, Birth_Date=:birth," +
            " Role=:role, Game_Id=:game_id, Team_Id=:team_id WHERE Id=:id";
        public static String SQL_UPDATE_PRESTUP = "UPDATE Person SET Team_Id=:team_id WHERE Id=:id";
        public static String SQL_DELETE_ID = "DELETE FROM Person WHERE Id=:id";
        public static String SQL_SELECT = "SELECT * FROM Person";
        public static String SQL_SELECT_ONE = "SELECT * FROM Person WHERE Id=:id";
        public static String SQL_SELECT_PAR = "SELECT * FROM Person WHERE Last_Name=:lname";
        public static String SQL_SELECT_TEAMMATES = "SELECT * FROM Person WHERE Id!=:id AND Team_Id=:tID";
        public static String SQL_UPDATE_GAME = "UPDATE Person SET GAME_ID=:game WHERE TEAM_ID=:team";


        private static void PrepareCommand(OracleCommand command, Person p)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":id", p.Id);
            command.Parameters.AddWithValue(":fname", p.First_Name);
            command.Parameters.AddWithValue(":lname", p.Last_Name);
            command.Parameters.AddWithValue(":birth", p.Birth_Date);
            if (p.Role == null)
            {
                command.Parameters.AddWithValue(":role", " ");
            }
            else
            {
                command.Parameters.AddWithValue(":role", p.Role);
            }
            command.Parameters.AddWithValue(":game_id", p.Game_Id.Id);
            command.Parameters.AddWithValue(":team_id", p.Team_Id.Id);

        }

        private static Collection<Person> Read(OracleDataReader reader)
        {
            Collection<Person> People = new Collection<Person>();

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
            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, p);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Update(Person p, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, p);
            
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }
        public static int UpdateGame(int tID, int gID, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_UPDATE_GAME);

            command.Parameters.AddWithValue(":team", gID);
            command.Parameters.AddWithValue(":game", tID);
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

            OracleCommand command = db.CreateCommand(SQL_UPDATE_PRESTUP);
            
            command.Parameters.AddWithValue(":team_id", tID);
            command.Parameters.AddWithValue(":id", pID);
            int ret = db.ExecuteNonQuery2(command);

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
            OracleCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue(":id", id);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }


        public static Collection<Person> Select(DatabaseT pDb = null)
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

            Collection<Person> Users = Read(reader);
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_ONE);
            command.Parameters.AddWithValue(":id", pID);
            OracleDataReader reader = db.Select(command);

            Person Users = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static Collection<Person> SelectByParameter(string lname = null, DatabaseT pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_PAR);
            command.Parameters.AddWithValue(":lname", lname);
            OracleDataReader reader = db.Select(command);

            Collection <Person> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static Collection<Person> SelectTeammates(int pID, DatabaseT pDb = null)
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
            
            
            OracleCommand command = db.CreateCommand(SQL_SELECT_TEAMMATES);
            command.Parameters.AddWithValue(":id", pID);
            command.Parameters.AddWithValue(":tID", tmp.Team_Id.Id);

            OracleDataReader reader = db.Select(command);

            Collection<Person> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }


    }
}
