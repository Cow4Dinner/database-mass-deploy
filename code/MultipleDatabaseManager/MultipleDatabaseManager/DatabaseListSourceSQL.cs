using System.Collections.Generic;
using System.Data.SqlClient;

namespace MultipleDatabaseManager
{
    internal class DatabaseListSourceSQL : IDatabaseListSource
    {
        private string sql;

        public DatabaseListSourceSQL(string sql)
        {
            this.sql = sql;
        }

        public IDictionary<string, SqlConnectionStringBuilder> Databases { get; set; }
        public string DefaultDatabaseName { get; set; }

        public List<SqlConnectionStringBuilder> process()
        {
            throw new System.NotImplementedException();
        }
    }
}