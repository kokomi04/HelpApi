using Infrastructures.AppConfigs.Model;

namespace Infrastructures.AppConfigs.Model
{
    public class AppSetting
    {
        public string ExternalHelpApiKey { get; set; }
        public ConfigurationSetting Configuration { get; set; }
        public DatabaseConnectionSetting DatabaseConnections { get; set; }
        public IdentitySetting Identity { get; set; }
        public AuthenticationSetting Authentication { get; set;}
    }
}
