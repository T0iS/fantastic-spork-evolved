using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Database.FunctionalityClasses
{
    public static class Extensions
    {
        public static void AddWithValue(this SqlParameterCollection cmd, string parameterName, object value)
        {
            cmd.Add(parameterName, value);
        }
    }
}
