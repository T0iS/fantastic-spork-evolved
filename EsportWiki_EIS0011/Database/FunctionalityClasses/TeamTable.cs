using System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace DataLayer.Database.FunctionalityClasses
{
    public class TeamTable
    {
        public static String SQL_INSERT = "INSERT INTO Team (Id, Name, Person_Id, Address_Id, Game_Id, Organisation_Id) VALUES " +
                "(@id, @name, @pID, @aID, @gID, @oID)";
        public static String SQL_UPDATE = "UPDATE Team SET Name=@name, Person_Id=@pID, Address_Id=@aID, Game_Id=@gID," +
            " Organisation_Id=@oID WHERE ID=@id";
        public static String SQL_UPDATE_ORG = "UPDATE Team SET Organisation_Id=@voID WHERE Id=@id";
        public static String SQL_DELETE_ID = "DELETE FROM Team WHERE Id=@id";
        public static String SQL_SELECT = "SELECT * FROM Team";
        public static String SQL_SELECT_ONE = "SELECT * FROM Team WHERE Id=@id";
        public static String SQL_SELECT_ORG = "SELECT Organisation_Id FROM Team WHERE Id=@id";
        public static String SQL_SELECT_COUNT = "SELECT count(id) from Team where Game_Id=@gID";
        public static String SQL_SELECT_PARAM = "SELECT * FROM Team WHERE Name=@name";
        


        private static void PrepareCommand(SqlCommand command, Team t)
        {
            
            command.Parameters.AddWithValue("@id", t.Id);
            command.Parameters.AddWithValue("@name", t.Name);
            if (t.Person_Id == null)
            {
                command.Parameters.AddWithValue("@pID", null);
            }
            else
            {
                command.Parameters.AddWithValue("@pID", t.Person_Id.Id);
            }
            command.Parameters.AddWithValue("@aID", t.Address_Id.Id);
            command.Parameters.AddWithValue("@gID", t.Game_Id.Id);
            command.Parameters.AddWithValue("@oID", t.Organisation_Id.Id);
            

        }

        private static List<Team> Read(SqlDataReader reader)
        {
            List<Team> Teams = new List<Team>();

            while (reader.Read())
            {
                int i = -1;
                Team m = new Team();
                m.Id = reader.GetInt32(++i);
                m.Name = reader.GetString(++i);
                
                if (!reader.IsDBNull(++i))
                {
                    Person p = new Person();
                    p.Id = reader.GetInt32(i);
                    p = PersonTable.SelectOne(p.Id);
                    m.Person_Id = p;    
                }
               
                Address a = new Address();
                a.Id = reader.GetInt32(++i);
                m.Address_Id = a;
                

                Game g = new Game();
                g.Id = reader.GetInt32(++i);
                g = GameTable.SelectOne(g.Id);
                m.Game_Id = g;

                if (!reader.IsDBNull(++i))
                {
                    Organisation t = new Organisation();
                    t.Id = reader.GetInt32(i);
                    t = OrganisationTable.SelectOne(t.Id);
                    m.Organisation_Id = t;
                }

                Teams.Add(m);
            }
            return Teams;
        }

        private static List<Organisation> ReadOrg(SqlDataReader reader)
        {
            List<Organisation> Organisations = new List<Organisation>();

            while (reader.Read())
            {
                int i = -1;
                Organisation m = new Organisation();
                m.Id = reader.GetInt32(++i);
               

                Organisations.Add(m);
            }
            return Organisations;
        }

        private static int ReadCount(SqlDataReader reader)
        {
            int res = 0;

            while (reader.Read())
            {
                int i = -1;
                res += reader.GetInt32(++i);

            }
            return res;
        }


        public static int Insert(Team t, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, t);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Update(Team t, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, t);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        public static int UpdateOrg(Team t, int oID, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();

            int count = 0;
            try
            {
                count = OrganisationTable.SelectTeamCountGame(oID, t.Game_Id.Id);
            }
            catch( Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            if (count > 0)
            {
                Console.WriteLine("Organizace uz ma tym pro tuto hru");
                return -1;
            }
            
            
            SqlCommand command = db.CreateCommand(SQL_UPDATE_ORG);
            command.Parameters.AddWithValue("@id", t.Id);
            command.Parameters.AddWithValue("@voID", oID);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
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


        public static List<Team> Select(DatabaseT pDb = null)
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

            List<Team> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static Team SelectOne(int tID, DatabaseT pDb = null)
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
            command.Parameters["@id"].Value = tID;

            SqlDataReader reader = db.Select(command);

            Team g = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return g;
        }

        public static List<Team> SelectByParam(string name = null, DatabaseT pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_PARAM);
            command.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = db.Select(command);

            List<Team> g = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return g;
        }

        public static Organisation SelectOrganisation(int pID, DatabaseT pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_ORG);
            command.Parameters.AddWithValue("@id", pID);
            SqlDataReader reader = db.Select(command);

            Organisation Users = ReadOrg(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }


        public static int SelectCountGame(int gID, DatabaseT pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_COUNT);
            command.Parameters.AddWithValue("@gID", gID);
            SqlDataReader reader = db.Select(command);

            int res = ReadCount(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return res;
        }


        public static int ZmenitHru(Team t, int gID, DatabaseT pDb = null)
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
            db.BeginTransaction();

            if (SelectOne(t.Id) == null)
            {
                db.Rollback();
                return -1;
            }
            if(GameTable.SelectOne(gID) == null)
            {
                db.Rollback();
                return -1;
            }

            try
            {
                PersonTable.UpdateGame(t.Id, gID);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                db.Rollback();
                return -1;
            }

            try
            {
                t.Game_Id = GameTable.SelectOne(gID);
                Update(t);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                db.Rollback();
                return -1;
            }


            SqlCommand command = db.CreateCommand(SQL_SELECT_COUNT);
            command.Parameters.AddWithValue("@gID", gID);
            SqlDataReader reader = db.Select(command);

            int res = ReadCount(reader);
            reader.Close();

            db.EndTransaction();
            if (pDb == null)
            {
                db.Close();
            }

            return res;
        }
    }

       

    
}
