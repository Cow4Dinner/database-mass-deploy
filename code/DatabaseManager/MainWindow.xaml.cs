using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace DatabaseManager
{
	using System.Collections.ObjectModel;
	using System.Data.SqlClient;

	using DatabaseTools;


	public class SqlObjects
	{
		public SqlObject defaultSqlObject = new SqlObject("Report_PatronValue");

		public ObservableCollection<SqlObject> sqlObjectsList
		{
			get
			{
				var helper = new SqlData();
				return helper.Get();
			}
			set
			{
			}

		}

	}


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{ 
		public MainWindow()
		{
			InitializeComponent();

			dbObjectField.DataContext = new SqlObjects();
		}


		private async void GoButton_Click(object sender, RoutedEventArgs e)
		{
			string searchedObjectName = dbObjectField.Text;
			string serverName = serverNameField.Text;

			try
			{
				var hashMassDataProcessor = new HashMassDataProcessor(searchedObjectName);
				var progress = new Progress<int>(p => this.progressBar.Value = p);

				List<dataRows> resultList = await DatabaseTools.massConnect<dataRows>(serverName, hashMassDataProcessor, progress);

				resultList =
					resultList.OrderByDescending(x => x.objectName)
						.ThenBy(x => x.hashValue)
						.ThenBy(x => x.showareVersion)
						.ThenBy(x => x.dbName)
						.ToList();
				
				MainDataGrid.DataContext = resultList;
			}
			finally
			{
				GoButton.IsEnabled = true;
			}
		}
	}
}
