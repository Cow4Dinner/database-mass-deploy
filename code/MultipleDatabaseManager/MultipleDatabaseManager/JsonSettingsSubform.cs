using System.Data.SqlClient;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using ScintillaNET;

namespace MultipleDatabaseManager
{
    /// <summary>
    /// Interaction logic for JSON_settings_subform.xaml
    /// </summary>
    public partial class JsonSettingsSubform : UserControl
    {
        public JsonSettingsSubform()
        {
            InitializeComponent();

           TextEditor.EnableSqlHighlighting(this.SourceJson);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            UserSettings.DefaultSourceJson = this.SourceJson.Text;

            UserSettings.DefaultSourceType = "json";

            UserSettings.Save();
        }


    }
}
