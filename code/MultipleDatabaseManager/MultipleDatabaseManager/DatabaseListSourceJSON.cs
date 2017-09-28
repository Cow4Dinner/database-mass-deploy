using System.Collections.Generic;
using System.Data.SqlClient;

namespace MultipleDatabaseManager
{
    internal class DatabaseListSourceJSON : IDatabaseListSource
    {
        private string json;

        public DatabaseListSourceJSON(string json)
        {
            this.json = json;
        }

        public IDictionary<string, SqlConnectionStringBuilder> Databases { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string DefaultDatabaseName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public List<SqlConnectionStringBuilder> process()
        {
            throw new System.NotImplementedException();
        }
    }
}