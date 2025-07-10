using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Helpers
{
	public class DBConnectionManager
	{
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}