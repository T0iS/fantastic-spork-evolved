using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataLayer.Database.FunctionalityClasses
{
    public class System_userTable
    {
        public static String SQL_INSERT = "INSERT INTO p_system_user (Id, Username, Rights) VALUES (@id, @username, @rights)";
        public static String SQL_UPDATE = "UPDATE p_system_user SET Username=@username, Rights=@rights WHERE Id=@id";
        public static String SQL_DELETE_ID = "DELETE FROM p_system_user WHERE Id=@id";
        

        private static void PrepareCommand(SqlCommand command, System_user user)
        {
           
            command.Parameters.AddWithValue("@id", user.Id);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@rights", user.Rights);
        }

       

        public static int Insert(System_user user, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, user);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Update(System_user user, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, user);
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
