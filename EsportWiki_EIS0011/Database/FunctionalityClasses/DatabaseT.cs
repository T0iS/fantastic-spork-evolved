using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;


namespace EsportWiki_EIS0011.Database.FunctionalityClasses
{
    public class DatabaseT
    {
        private OracleConnection Connection { get; set; }
        private OracleTransaction SqlTransaction { get; set; }
        public string Language { get; set; }

        public DatabaseT()
        {
            Connection = new OracleConnection();
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
                ret = Connect(ConfigurationManager.ConnectionStrings["ConnectionStringOracle"].ConnectionString);
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
        public int ExecuteNonQuery(OracleCommand command)
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

        public int ExecuteNonQuery2(OracleCommand command)
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
            
            return rowNumber;
        }

        /// <summary>
        /// Create command.
        /// </summary>
        public OracleCommand CreateCommand(string strCommand)
        {
            OracleCommand command = new OracleCommand(strCommand, Connection);

            if (SqlTransaction != null)
            {
                command.Transaction = SqlTransaction;
            }
            return command;
        }
        /// <summary>
        /// Select encapulated in the command.
        /// </summary>
        public OracleDataReader Select(OracleCommand command)
        {
            //command.Prepare();
            OracleDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }
    }
}
