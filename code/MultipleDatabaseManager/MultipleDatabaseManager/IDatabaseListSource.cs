using System.Collections.Generic;
using System.Data.SqlClient;

namespace MultipleDatabaseManager
{
    internal interface IDatabaseListSource
    {
        List<SqlConnectionStringBuilder> process();
        IDictionary<string, SqlConnectionStringBuilder> Databases { get; set; }
        string DefaultDatabaseName { get; set; }
    }
}