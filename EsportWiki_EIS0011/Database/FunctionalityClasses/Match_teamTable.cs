using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace DataLayer.Database.FunctionalityClasses
{
    public class Match_teamTable
    {
        public static String SQL_INSERT = "INSERT INTO Match_team (Match_Id, Team_Id) VALUES @id, @team";
        public static String SQL_DELETE_ID = "DELETE FROM Event WHERE Match_Id=@id";
        


        private static void PrepareCommand(SqlCommand command, Match_team m)
        {
            
            command.Parameters.AddWithValue("@id", m.Match_Id);
            command.Parameters.AddWithValue("@team", m.Team_Id);
        }
        

        public static int Insert(Match_team m, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, m);
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


   


    }
}
