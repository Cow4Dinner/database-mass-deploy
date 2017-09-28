using System;
using System.Data.SqlClient;

namespace MultipleDatabaseManager
{
    internal class ConnectionFactory
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(UserSettings.DefaultSourceConnectionString);
        }
    }
}