using System.Configuration;

namespace MultipleDatabaseManager
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class SavedSource
    {
        public object Type { get; internal set; }
        public string Json { get; internal set; }
        public string Sql { get; internal set; }
    }
}