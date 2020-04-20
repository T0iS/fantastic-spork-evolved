using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;


namespace EsportWiki_EIS0011.Database.FunctionalityClasses
{
    class OrganisationTable
    {
        public static String SQL_INSERT = "INSERT INTO Organisation (Id, Name, Person_Id, Address_Id) VALUES " +
                "(:id, :name, :pID, :aID)";
        public static String SQL_UPDATE = "UPDATE Organisation SET Name=:name, Address_Id=:aID WHERE Id=:id";
        public static String SQL_UPDATE_MAN = "UPDATE Organisation SET Person_Id=:pID WHERE Id=:id";
        public static String SQL_SELECT_TEAM_COUNT = "SELECT count(Id) from Team WHERE organisation_id=:oID";
        public static String SQL_SELECT_TEAM_COUNT_gamePAR = "SELECT count(Id) from Team WHERE organisation_id=:oID AND Game_Id=:gID";
        public static String SQL_SELECT_ONE = "SELECT * from Organisation WHERE Id=:oID";
        public static String SQL_SELECT_P = "SELECT * from Person JOIN Team on Team.Id = Person.Team_Id WHERE Team.Organisation_Id = :id";
        public static String SQL_DELETE_ID = "DELETE FROM Organisation WHERE Id =:id";

        private static void PrepareCommand(OracleCommand command, Organisation o)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":id", o.Id);
            command.Parameters.AddWithValue(":name", o.Name);
            command.Parameters.AddWithValue(":pID", o.Person_Id.Id);
            command.Parameters.AddWithValue(":aID", o.Address_Id.Id);
           

        }

        private static Collection<Person> ReadP(OracleDataReader reader)
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
                m.Game_Id = g;

                Team t = new Team();
                t.Id = reader.GetInt32(++i);
                m.Team_Id = t;
               
                People.Add(m);
            }
            return People;
        }
    
        private static Collection<Organisation> Read(OracleDataReader reader)
        {
            Collection<Organisation> Organisations = new Collection<Organisation>();

            while (reader.Read())
            {
                int i = -1;
                Organisation m = new Organisation();
                m.Id = reader.GetInt32(++i);
                m.Name = reader.GetString(++i);
                
                if (!reader.IsDBNull(++i))
                {
                    Person p = new Person();
                    p.Id = reader.GetInt32(i);
                    m.Person_Id = p;
                }
                
                Address g = new Address();
                g.Id = reader.GetInt32(++i);
                m.Address_Id = g;

               

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


        public static int Insert(Organisation o, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, o);
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


        public static int Update(Organisation o, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, o);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        public static int UpdateManager(Organisation o, int pID, bool is_manager, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();

            db.BeginTransaction();
            if (SelectOne(o.Id) == null)
            {
                Console.WriteLine("Organizace nenalezena");
                db.Rollback();
                return -1;
            }
            if(PersonTable.SelectOne(pID) == null)
            {
                Console.WriteLine("Osoba nenalezena");
                db.Rollback();
                return -1;
            }

            if(o.Person_Id.Id == pID)
            {
                Console.WriteLine("Osoba jiz je kontaktni osobou organizace");
                db.Rollback();
                return -1;
            }

            try
            {
                Person p = new Person();
                p = PersonTable.SelectOne(pID);

                if (is_manager)
                {
                    p.Role = "Manager";
                }
                else
                {
                    p.Role = null;
                }
                PersonTable.Update(p);

                o.Person_Id.Id = pID;
                Update(o);    

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                db.Rollback();
                return -1;
            }


            OracleCommand command = db.CreateCommand(SQL_UPDATE_MAN);
            PrepareCommand(command, o);
            int ret = db.ExecuteNonQuery2(command);
            db.EndTransaction();
            db.Close();
            return ret;
        }




        public static Collection<Person> SelectPeople(int oID, DatabaseT pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_P);
            command.Parameters.AddWithValue(":id", oID);
            OracleDataReader reader = db.Select(command);

            Collection<Person> Users = ReadP(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static int SelectTeamCount(int oID, DatabaseT pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_TEAM_COUNT);
            command.Parameters.AddWithValue(":oID", oID);
            OracleDataReader reader = db.Select(command);

            int res = ReadCount(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return res;
        }


        public static int SelectTeamCountGame(int oID, int gID, DatabaseT pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_TEAM_COUNT_gamePAR);
            command.Parameters.AddWithValue(":oID", oID);
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

      


        public static Organisation SelectOne(int pID, DatabaseT pDb = null)
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

            Organisation Users = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }


    }
}
