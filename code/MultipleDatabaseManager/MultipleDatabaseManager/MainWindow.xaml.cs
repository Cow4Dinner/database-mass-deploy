using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.Caching;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MultipleDatabaseManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            /*Generated code*/
            InitializeComponent();

         

            /*Initialize Tabs*/
            InitializeSettingsTab();

            /*Select a tab to start on*/
            SelectInitialTab();
        }

        private void SelectInitialTab()
        {
            ObjectCache cache = MemoryCache.Default;
            List<SqlConnectionStringBuilder> dbList = (List<SqlConnectionStringBuilder>)cache["databaseList"];

            /*If no current list switch to settings tab. Otherwise start on mass deploy object tab*/
            if (dbList?.Count < 1)
            {
                Dispatcher.BeginInvoke((Action)(() => this.mainTabControl.SelectedItem = SettingsTab));
            }
            else
            {
                Dispatcher.BeginInvoke((Action)(() => this.mainTabControl.SelectedItem = MassDeployObjectTab));
            }
        }

        

        private void InitializeSettingsTab() {
            /*Load settings into cache*/
            LoadSettingsIntoCache();

            InitializeSourceTypeValues();
        }

        

        private void InitializeSourceTypeValues() {
            ObservableCollection<string> list = new ObservableCollection<string> {"Database", "JSON"};
            this.SourceTypeComboBox.ItemsSource = list;
            this.SourceTypeComboBox.SelectedIndex = 0;
            /*Make sure that index 0's page is actually shown on start because of this*/
        }



        private void LoadSettingsIntoCache()
        {
            IDatabaseListSource dbSource = default(IDatabaseListSource);

            /*Try and get a saved source.*/
            if (UserSettings.HasSavedSource)
            {
                /*run the processor of any saved source to refresh list*/
                switch (UserSettings.DefaultSourceType.ToLower())
                {
                    case "database":
                        dbSource = new DatabaseListSourceSQL(UserSettings.DefaultSourceSql);
                        break;

                    case "json":
                        dbSource = new DatabaseListSourceJSON(UserSettings.DefaultSourceJson);
                        break;
                }

                /*If source has databases save them to cache and load*/
                if (dbSource?.Databases?.Count > 0)
                {
                    ObjectCache cache = MemoryCache.Default;
                    cache.Add("databaseList", dbSource.Databases, DateTimeOffset.MaxValue);
                    cache.Add("defaultSourceDatabase", dbSource.DefaultDatabaseName, DateTimeOffset.MaxValue);
                }
            }
        }

        /*Actions*/
        private void SourceChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSource = SourceTypeComboBox.SelectedItem.ToString();
            SettingsSubform.Source = new Uri(selectedSource + "_settings_subform.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
