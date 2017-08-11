using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseTools
{
	using System.Data.SqlClient;
	using System.Diagnostics;
	using System.Dynamic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	public class HashMassDataProcessor : IMassDataProcessor<dataRows>
	{
		string searchedObjectName;

		public HashMassDataProcessor(string searchedObjectName = "Report_PatronValue")
		{
			this.searchedObjectName = searchedObjectName;
		}

		public dataRows getRow(string currentDatabaseServer, string dbName, dynamic parameters = null)
		{
			string clientConnectionString = DatabaseTools.buildConnectionString(currentDatabaseServer, dbName);
			List<dynamic> hashesList = new List<dynamic>();

			using (SqlConnection dbconn = new SqlConnection(clientConnectionString))
			{
				string query = @"

					DECLARE @spname Nvarchar(50)
					DECLARE @spdefinition Nvarchar(MAX)
					DECLARE @defModified Nvarchar(MAX)
					DECLARE @hashedVal varbinary(MAX)
					DECLARE @chunks AS TABLE(chunk Nvarchar(MAX), sequence int)
					DECLARE @i int

					SET @spname = 'Report_PatronValue'
					SET @spdefinition = (SELECT OBJECT_DEFINITION (OBJECT_ID(@spname)))
					SET @defModified = @spdefinition
					SET @i = 0

					WHILE ((SELECT LEN(@defModified)) > 0)
					BEGIN
						SET @i = @i+1
						DECLARE @firstchunk AS Nvarchar(4000)
						SELECT @firstchunk = SUBSTRING(@defModified, 0, 4000)
						INSERT INTO @chunks VALUES(@firstchunk, @i)
						SELECT @defModified = SUBSTRING(@defModified, 4000, (SELECT LEN(@defModified)-LEN(@firstchunk)))
					END

					SELECT @spname AS ObjectName, (Select c.chunk AS [text()] From @chunks c ORDER BY c.sequence For XML PATH ('')) AS ObjectDefinition, SUBSTRING((SELECT '~' +  SUBSTRING(master.dbo.fn_varbintohexstr(HashBytes('SHA1', chunk)), 3, 32)  FROM @chunks FOR XML PATH('')),2,200000) AS HashValue, (SELECT tsgs.ShoWareVersion FROM TicketSystem_GeneralSettings tsgs) AS ShowareVersion
				";

				query = query.Replace("{OBJECT_ID}", this.searchedObjectName);


				using (SqlCommand objectHashList = new SqlCommand(query, dbconn))
				{
					try
					{
						dbconn.Open();
						try
						{
							using (SqlDataReader hashReader = objectHashList.ExecuteReader())
							{
								var dbHash = "";
								var objectDefinition = "";
								Dictionary<string, dynamic> sqlObject;
								string newHash = String.Empty;
								byte[] bytes;

								while (hashReader.Read())
								{
									dbHash = hashReader["HashValue"].ToString();
									objectDefinition = hashReader["ObjectDefinition"].ToString();
									sqlObject = new Dictionary<string, dynamic>();

									if (sqlObject.ContainsKey(dbHash))
									{
										sqlObject[dbHash].count += 1;
									}
									else
									{
										newHash = String.Empty;
										bytes = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(objectDefinition), 0, Encoding.UTF8.GetByteCount(objectDefinition));

										foreach (byte theByte in bytes)
										{
											newHash += theByte.ToString("x2");
										}

										sqlObject[dbHash] = new ExpandoObject();
										sqlObject[dbHash].count = 1;
										sqlObject[dbHash].objectDefinition = objectDefinition;
										sqlObject[dbHash].objectName = hashReader["ObjectName"].ToString();
										sqlObject[dbHash].objectHashValue = newHash;
										sqlObject[dbHash].dbObjectHashValue = dbHash;
									}

									dynamic newItem = new ExpandoObject();

									newItem.dbName = dbName;
									newItem.showareVersion = hashReader["ShowareVersion"].ToString();
									newItem.data = sqlObject[dbHash];

									hashesList.Add(newItem);
								}
							}
						}
						catch (Exception e)
						{
							Trace.WriteLine("Failed to read data \r\n" + dbName + " \r\n" + e.Source);
						}
					}
					catch (Exception e)
					{
						Trace.WriteLine("Failed to open " + dbName + " \r\n" + e.Source + "\r\n" + clientConnectionString);
					}
				}
			}
			dynamic obj = hashesList.FirstOrDefault();

			if (obj != null)
			{
				string objectName = obj.data.objectName;
				string hash = obj.data.objectHashValue;
				dynamic theObject = obj.data;

				return new dataRows(objectName, hash, obj.dbName, obj.showareVersion, theObject);
			}
			
			return null;
			
		}

		


	}




}
