using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;

namespace MultipleDatabaseManager
{
    internal static class UserSettings
    {
        public static bool HasSavedSource { get; internal set; }
        public static string DefaultSourceConnectionString {
            get => (string) Properties.Settings.Default["DefaultSourceConnectionString"].ToString();
            set => Properties.Settings.Default["DefaultSourceConnectionString"] = (string) value;
        }
        public static string DefaultSourceSql {
            get => (string)Properties.Settings.Default["DefaultSourceSql"].ToString();
            set => Properties.Settings.Default["DefaultSourceSql"] = (string)value;
        }
        public static string DefaultSourceType {
            get => (string)Properties.Settings.Default["DefaultSourceType"].ToString();
            set => Properties.Settings.Default["DefaultSourceType"] = (string)value;
        }
        public static string DefaultSourceJson {
            get => (string)Properties.Settings.Default["DefaultSourceJson"].ToString();
            set => Properties.Settings.Default["DefaultSourceJson"] = (string)value;
        }

        private static List<SqlConnectionStringBuilder> sourceDatabases = new List<SqlConnectionStringBuilder>();

        public static List<SqlConnectionStringBuilder> SourceDatabases
        {
            get
            {
                switch (DefaultSourceType)
                {
                    case "database":
                        if (sourceDatabases.Count == 0)
                        {
                            LoadDefaultDatabases();
                        }
                        return sourceDatabases;
                    case "json":
                        return null;
                    default:
                        return null;
                }
            }
            set
            {
                if (value.Count == 0)
                    throw new ArgumentException("Value cannot be an empty collection.", nameof(value));
                sourceDatabases = value;
            }
        }

        static UserSettings()
        {
            Properties.Settings.Default.Upgrade();

            HasSavedSource = DefaultSourceConnectionString != String.Empty;
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        public static async Task LoadDefaultDatabases()
        {
            List<SqlConnectionStringBuilder> databaseList = new List<SqlConnectionStringBuilder>();

            using (var connection = ConnectionFactory.GetConnection())
            {
                try
                {
                    databaseList = (await connection.QueryAsync<SqlConnectionStringBuilder>(DefaultSourceSql)).ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to connect to database. Check connection string properties.");

                }
            }

            SourceDatabases = databaseList;
        }
    }
}