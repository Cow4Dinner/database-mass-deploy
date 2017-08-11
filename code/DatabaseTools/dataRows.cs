namespace DatabaseTools
{
	public class dataRows
	{
		public string objectName { get; set; }
		public string hashValue { get; set; }
		public string dbName { get; set; }
		public string showareVersion { get; set; }
		public dynamic data { get; set; }

		public dataRows(string objectName = "", string hashValue = "", string dbName = "", string showareVersion = "", dynamic data = null)
		{
			this.objectName = objectName;
			this.hashValue = hashValue;
			this.dbName = dbName;
			this.showareVersion = showareVersion;
			this.data = data;
		}

	}
}