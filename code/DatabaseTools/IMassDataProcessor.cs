namespace DatabaseTools
{
	public interface IMassDataProcessor<T>
	{
		T getRow(string currentDatabaseServer, string dbName, dynamic parameters = null);
	}
}