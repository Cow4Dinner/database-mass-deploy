using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ScintillaNET;


namespace MultipleDatabaseManager
{
    /// <summary>
    /// Interaction logic for Database_settings_subform.xaml
    /// </summary>
    public partial class DatabaseSettingsSubform : UserControl
    {
        public DatabaseSettingsSubform()
        {
            InitializeComponent();

            TextEditor.EnableSqlHighlighting(this.SourceSql);

            SourceSql.Text = UserSettings.DefaultSourceSql;

            if (UserSettings.DefaultSourceConnectionString != "")
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(UserSettings.DefaultSourceConnectionString);
                ServerTextbox.Text = connectionStringBuilder.DataSource;
                NameTextbox.Text = connectionStringBuilder.InitialCatalog;
                UsernameTextbox.Text = connectionStringBuilder.UserID;
                PasswordTextbox.Text = connectionStringBuilder.Password;
            }

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder connectionStringBuilder =
                new SqlConnectionStringBuilder
                {
                    DataSource = ServerTextbox.Text,
                    InitialCatalog = NameTextbox.Text,
                    UserID = UsernameTextbox.Text,
                    Password = PasswordTextbox.Text
                };

            UserSettings.DefaultSourceConnectionString = connectionStringBuilder.ToString();

            UserSettings.DefaultSourceSql = this.SourceSql.Text;

            UserSettings.DefaultSourceType = "database";

            using (FileStream fs = File.Create("./res/save/source"))
            {
                //you can use the filstream here to put stuff in the file if you want to
                fs.WriteAsync(System.Text.Encoding.Unicode.GetBytes(SourceSql.Text), 0, SourceSql.TextLength+1);
            }
            

            UserSettings.Save();
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {

        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(UserSettings.SourceDatabases.Count > 0 ? "Records successfully pulled." : "Failed to pull records. Check SQL.",
                "Confirmation", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

    }
}
