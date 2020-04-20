using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;


namespace EsportWiki_EIS0011.Database.FunctionalityClasses
{
    class TeamTable
    {
        public static String SQL_INSERT = "INSERT INTO Team (Id, Name, Person_Id, Address_Id, Game_Id, Organisation_Id) VALUES " +
                "(:id, :name, :pID, :aID, :gID, :oID)";
        public static String SQL_UPDATE = "UPDATE Team SET Name=:name, Person_Id=:pID, Address_Id=:aID, Game_Id=:gID," +
            " Organisation_Id=:oID WHERE ID=:id";
        public static String SQL_UPDATE_ORG = "UPDATE Team SET Organisation_Id=:voID WHERE Id=:id";
        public static String SQL_DELETE_ID = "DELETE FROM Team WHERE Id=:id";
        public static String SQL_SELECT = "SELECT * FROM Team";
        public static String SQL_SELECT_ONE = "SELECT * FROM Team WHERE Id=:id";
        public static String SQL_SELECT_ORG = "SELECT Organisation_Id FROM Team WHERE Id=:id";
        public static String SQL_SELECT_COUNT = "SELECT count(id) from Team where Game_Id=:gID";
        public static String SQL_SELECT_PARAM = "SELECT * FROM Team WHERE Name=:name";
        


        private static void PrepareCommand(OracleCommand command, Team t)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":id", t.Id);
            command.Parameters.AddWithValue(":name", t.Name);
            if (t.Person_Id == null)
            {
                command.Parameters.AddWithValue(":pID", null);
            }
            else
            {
                command.Parameters.AddWithValue(":pID", t.Person_Id.Id);
            }
            command.Parameters.AddWithValue(":aID", t.Address_Id.Id);
            command.Parameters.AddWithValue(":gID", t.Game_Id.Id);
            command.Parameters.AddWithValue(":oID", t.Organisation_Id.Id);
            

        }

        private static Collection<Team> Read(OracleDataReader reader)
        {
            Collection<Team> Teams = new Collection<Team>();

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
                    m.Person_Id = p;    
                }
               
                Address a = new Address();
                a.Id = reader.GetInt32(++i);
                m.Address_Id = a;
                

                Game g = new Game();
                g.Id = reader.GetInt32(++i);
                m.Game_Id = g;

                if (!reader.IsDBNull(++i))
                {
                    Organisation t = new Organisation();
                    t.Id = reader.GetInt32(i);
                    m.Organisation_Id = t;
                }

                Teams.Add(m);
            }
            return Teams;
        }

        private static Collection<Organisation> ReadOrg(OracleDataReader reader)
        {
            Collection<Organisation> Organisations = new Collection<Organisation>();

            while (reader.Read())
            {
                int i = -1;
                Organisation m = new Organisation();
                m.Id = reader.GetInt32(++i);
               

                Organisations.Add(m);
            }
            return Organisations;
        }

        private static int ReadCount(OracleDataReader reader)
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
            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, t);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Update(Team t, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_UPDATE);
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
            
            
            OracleCommand command = db.CreateCommand(SQL_UPDATE_ORG);
            command.Parameters.AddWithValue(":id", t.Id);
            command.Parameters.AddWithValue(":voID", oID);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
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


        public static Collection<Team> Select(DatabaseT pDb = null)
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

            Collection<Team> Users = Read(reader);
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_ONE);
            command.Parameters.AddWithValue(":id", tID);
            OracleDataReader reader = db.Select(command);

            Team g = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return g;
        }

        public static Collection<Team> SelectByParam(string name = null, DatabaseT pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_PARAM);
            command.Parameters.AddWithValue(":name", name);
            OracleDataReader reader = db.Select(command);

            Collection<Team> g = Read(reader);
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_ORG);
            command.Parameters.AddWithValue(":id", pID);
            OracleDataReader reader = db.Select(command);

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

            OracleCommand command = db.CreateCommand(SQL_SELECT_COUNT);
            command.Parameters.AddWithValue(":gID", gID);
            OracleDataReader reader = db.Select(command);

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


            OracleCommand command = db.CreateCommand(SQL_SELECT_COUNT);
            command.Parameters.AddWithValue(":gID", gID);
            OracleDataReader reader = db.Select(command);

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
