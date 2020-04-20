using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace EsportWiki_EIS0011.Database.FunctionalityClasses
{
    public static class Extensions
    {
        public static void AddWithValue(this OracleParameterCollection cmd, string parameterName, object value)
        {
            cmd.Add(parameterName, value);
        }
    }
}
