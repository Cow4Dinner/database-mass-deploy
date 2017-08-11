using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;


namespace DatabaseTools
{
	public class SqlData
	{
		public SqlData(){}

		public ObservableCollection<SqlObject> Get()
		{
			ObservableCollection<SqlObject> objects = new ObservableCollection<SqlObject>();
			var dbconn = new SqlConnection(DatabaseTools.buildConnectionString("DEVSQL01", "ShoWareDevelopment_20228"));

			using (SqlCommand objectHashList = new SqlCommand("SELECT [schema] = OBJECT_SCHEMA_NAME([object_id]), name FROM sys.procedures ORDER BY name", dbconn))
			{
				try
				{
					dbconn.Open();
					try
					{
						using (SqlDataReader reader = objectHashList.ExecuteReader())
						{
							while (reader.Read())
							{
								objects.Add(new SqlObject(reader["name"].ToString()));
							}
						}
					}
					catch (Exception e)
					{
						Trace.WriteLine("Failed ");
					}
				}
				catch (Exception e)
				{
					Trace.WriteLine("Failed ");
				}
			}

			return objects;
		}
	}


	public class SqlObject
	{
		public SqlObject(string name)
		{
			this.Name = name;
		}

		public string Name { get; set; }
		public string FullName { get; set; }
	}
}
