using System.Collections.Generic;
using System.Data.SqlClient;

namespace MultipleDatabaseManager
{
    internal class DBSource
    {
        public List<SqlConnectionStringBuilder> Databases { get; set; }
        public SqlConnectionStringBuilder DefaultDatabase { get; set; }
        public string Sql { get; set; }
        
    }
}