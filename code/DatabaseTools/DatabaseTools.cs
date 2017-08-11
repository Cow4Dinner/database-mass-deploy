using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseTools
{
	using System.Data.SqlClient;
	using System.Diagnostics;

	public class DatabaseTools
	{
		public static string buildConnectionString(string serverName, string dbName = "master", string persistSecurityInfo = "true", string userName = "showareuser", string password = "ticketprinting")
		{
			string[] connectionString = new string[5];

			connectionString[0] = "Data Source=" + serverName;
			connectionString[1] = "Initial Catalog=" + dbName;
			connectionString[2] = "Persist Security Info=" + persistSecurityInfo;
			connectionString[3] = "User ID=" + userName;
			connectionString[4] = "Password=" + password;

			return string.Join(";", connectionString);
		}

		/*
		 Runs any function's worth of code against all of the showare databases on a given server
		 */
		public static async Task<List<T>> massConnect<T>(string currentDatabaseServer, IMassDataProcessor<T> dataProcessor, IProgress<int> progress)
		{
			string masterConnectionString = buildConnectionString(currentDatabaseServer);

			List<T> objs = new List<T>();

			using (SqlConnection conn = new SqlConnection(masterConnectionString))
			{
				using (SqlCommand dbList = new SqlCommand("SELECT (SELECT COUNT(*) FROM sysdatabases) AS NumberOfSystems, name AS DatabaseName FROM sysdatabases", conn))
				{
					try
					{
						await conn.OpenAsync();
						try
						{
							using (SqlDataReader dbNameReader = await dbList.ExecuteReaderAsync())
							{
								var tasks = new List<Task>();
								int numProcessed = 0;
								while (await dbNameReader.ReadAsync())
								{
									string dbName = dbNameReader["DatabaseName"].ToString();
									string numberOfSystems = dbNameReader["NumberOfSystems"].ToString();

									try
									{
										var task = Task.Run(() =>
										{
											T retVal = dataProcessor.getRow(currentDatabaseServer, dbName);

											if (retVal != null)
											{
												T obj = retVal;
												objs.Add(obj);
											}

											if (progress != null)
											{
												int NumberOfSystems = 0;
												int.TryParse(numberOfSystems, out NumberOfSystems);
												progress.Report(++numProcessed * 100 / NumberOfSystems);
											}

										});
										tasks.Add(task);
									}
									catch (Exception e)
									{
										Trace.WriteLine("Failed to Add Value . \r\n" + e.Source + "\r\n" + masterConnectionString);
									}

									await Task.WhenAll(tasks); ;

								}
							}
						}
						catch (Exception e)
						{
							Trace.WriteLine("Reading DBname Failed. \r\n" + e.Source + "\r\n" + masterConnectionString);
						}
					}
					catch (Exception e)
					{
						Trace.WriteLine("Failed to open " + masterConnectionString + " \r\n" + e.Source);
					}
				}
			}

			return objs;
		}
	}
}