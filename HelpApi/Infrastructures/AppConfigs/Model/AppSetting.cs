namespace Infrastructures.Commons.AppConfigs.Model
{
    public class AppSetting
    {
        public ConfigurationSetting Configuration { get; set; }
        public DatabaseConnectionSetting DatabaseConnections { get; set; }
        public IdentitySetting Identity { get; set; }
    }
}
