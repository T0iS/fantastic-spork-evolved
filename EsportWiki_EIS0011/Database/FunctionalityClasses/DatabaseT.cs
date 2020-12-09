using System;
using System.Data;
using System.Data.SqlClient;
using BusinessLayer;

namespace DataLayer.Database.FunctionalityClasses
{
    public class DatabaseT
    {
        private SqlConnection Connection { get; set; }
        private SqlTransaction SqlTransaction = null;
        public string Language { get; set; }

        private static String CONNECTION_STRING = "server=dbsys.cs.vsb.cz\\STUDENT;database=eis0011;user=eis0011;password=pO54vRpOCg;Connection Timeout=600;";
        //private static String CONNECTION_STRING = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Tom\\source\\repos\\EsportWiki_EIS0011\\EsportWiki_EIS0011\\Database.mdf;Integrated Security=True;Connection Timeout=600";


        public DatabaseT()
        {
            Connection = new SqlConnection();
            Language = "en";
        }

        /// <summary>
        /// Connect
        /// </summary>
        public bool Connect(String conString)
        {
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Connection.ConnectionString = conString;
                Connection.Open();
            }
            return true;
        }

        /// <summary>
        /// Connect
        /// </summary>
        public bool Connect()
        {
            bool ret = true;

            if (Connection.State != System.Data.ConnectionState.Open)
            {
                //ret = Connect(ConfigurationManager.ConnectionStrings["ConnectionStringOracle"].ConnectionString);
                ret = Connect(CONNECTION_STRING);
            }

            return ret;
        }

        /// <summary>
        /// Close.
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Begin a transaction.
        /// </summary>
        public void BeginTransaction()
        {
            SqlTransaction = Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        /// <summary>
        /// End a transaction.
        /// </summary>
        public void EndTransaction()
        {
            //command.Dispose()
            SqlTransaction.Commit();
            Close();
        }

        /// <summary>
        /// If a transaction is failed call it.
        /// </summary>
        public void Rollback()
        {
            SqlTransaction.Rollback();
        }

        /// <summary>
        /// Insert a record encapulated in the command.
        /// </summary>
        public int ExecuteNonQuery(SqlCommand command)
        {
            int rowNumber = 0;
            try
            {
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Close();
            }
            return rowNumber;
        }

        /// <summary>
        /// Create command.
        /// </summary>
        public SqlCommand CreateCommand(string strCommand)
        {
            SqlCommand command = new SqlCommand(strCommand, Connection);

            if (SqlTransaction != null)
            {
                command.Transaction = SqlTransaction;
            }
            return command;
        }
        /// <summary>
        /// Select encapulated in the command.
        /// </summary>
        public SqlDataReader Select(SqlCommand command)
        {
            command.Prepare();
            SqlDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }

    }
}
