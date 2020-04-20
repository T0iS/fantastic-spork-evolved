using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;

namespace EsportWiki_EIS0011.Database.FunctionalityClasses
{
    class EventTable
    {
        public static String SQL_INSERT = "INSERT INTO Event (Id, Name, Organiser, Prizepool) VALUES (:id, :name, :organiser, :pp)";
        public static String SQL_UPDATE = "UPDATE Event SET Name=:name, Organiser=:organiser, Prizepool=:pp WHERE Id=:id";
        public static String SQL_DELETE_ID = "DELETE FROM Event WHERE Id=:id";
        public static String SQL_SELECT = "SELECT * FROM Event order by Name asc";
        public static String SQL_SELECT_ONE = "SELECT * FROM Event WHERE Id=:id";
        public static String SQL_SELECT_LAST = "SELECT MAX(Id) FROM Event";
        public static String SQL_SELECT_ONE_PAR = "SELECT * FROM Event WHERE Organiser=:org";


        private static void PrepareCommand(OracleCommand command, Event e)
        {
            command.BindByName = true;
            command.Parameters.AddWithValue(":id", e.Id);
            command.Parameters.AddWithValue(":name", e.Name);
            command.Parameters.AddWithValue(":organiser", e.Organiser);
            command.Parameters.AddWithValue(":pp", e.Prizepool);
        }

        private static Collection<Event> Read(OracleDataReader reader)
        {
            Collection<Event> Events = new Collection<Event>();

            while (reader.Read())
            {
                int i = -1;
                Event g = new Event();
                g.Id = reader.GetInt32(++i);
                g.Name = reader.GetString(++i);
                g.Organiser = reader.GetString(++i);

                if (!reader.IsDBNull(++i))
                {
                    g.Prizepool = reader.GetInt32(i);
                }
                Events.Add(g);
            }
            return Events;
        }
        private static int ReadLast(OracleDataReader reader)
        {
           
            int ret = 0;
            while (reader.Read())
            {
                int i = -1;
                
                ret = reader.GetInt32(++i);
                
                
            }
            return ret;
        }


        public static int Insert(Event e, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, e);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }
        
        public static int PridatAkci(Event e, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();

            int event_id = 0;
            if (e.Prizepool == null)
            {
                e.Prizepool = 0;
            }

            if (SelectAllFromOrganiser(e.Organiser).Count > 500)
            {
                Console.WriteLine("Prekrocen maximalni pocet akci");
                return -1;
            }

            event_id = SelectLast();
            e.Id = ++event_id;


            OracleCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, e);
            int ret = 0;
            try
            {
                ret = db.ExecuteNonQuery(command);
            }
            catch(Exception er)
            {
                Console.WriteLine(er.Message);
            }

            db.Close();
            return ret;


        }


        public static int Update(Event e, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            OracleCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, e);
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


        public static Collection<Event> Select(DatabaseT pDb = null)
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

            Collection<Event> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static Event SelectOne(int pID, DatabaseT pDb = null)
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

            Event Users = Read(reader).First();
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }

        public static Collection<Event> SelectAllFromOrganiser(string or, DatabaseT pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_ONE_PAR);
            command.Parameters.AddWithValue(":org", or);
            OracleDataReader reader = db.Select(command);

            Collection<Event> Users = Read(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Users;
        }
        public static int SelectLast(DatabaseT pDb = null)
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

            OracleCommand command = db.CreateCommand(SQL_SELECT_LAST);
            
            OracleDataReader reader = db.Select(command);

            int res = ReadLast(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return res;
        }



       
    }
}
