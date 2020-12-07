using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Database.FunctionalityClasses
{
    public class InquiryTable
    {
        public static String SQL_INSERT = "INSERT INTO inquiries (inq_desc, inq_text) VALUES (@inq_desc, @inq_text)";

        private static void PrepareCommand(SqlCommand command, Inquiry inquiry)
        {

            command.Parameters.Add(new SqlParameter("@inq_desc", SqlDbType.VarChar));
            command.Parameters["@inq_desc"].Value = inquiry.desc;

            command.Parameters.Add(new SqlParameter("@inq_text", SqlDbType.VarChar));
            command.Parameters["@inq_text"].Value = inquiry.text;

        }

        public static int Insert(Inquiry inquiry, DatabaseT pDb = null)
        {
            DatabaseT db = new DatabaseT();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, inquiry);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }











    }
}
