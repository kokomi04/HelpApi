﻿using Infrastructures.AppConfigs.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Infrastructures.AppConfigs
{
    public class AppConfigSetting
    {
        private AppConfigSetting() { }

        public IConfigurationRoot Configuration { get; private set; }
        public AppSetting AppSetting { get; private set; }

        public static AppConfigSetting Config(string environmentName = null, bool excludeSensitiveConfig = false, string basePath = null)
        {
            var exeFolder = basePath ?? Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase) ?? string.Empty;
            if (EnviromentConfig.IsUnitTest)
            {
                exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            }

            exeFolder = exeFolder
                .Replace(@"file:\", "")
                .Replace(@"file:", "")
                .TrimStart('/');

            var modeName = EnviromentConfig.EnviromentName;

            var builder = new ConfigurationBuilder()
                    .SetBasePath(exeFolder)
                    .AddJsonFile($"AppSetting.json", true, true)
                    .AddJsonFile($"AppSetting.{environmentName ?? modeName}.json", false, true);

            if (!excludeSensitiveConfig)
            {
                AddEnvironmentConfig(builder);
            }

            var config = builder.Build();


            var result = new AppConfigSetting();
            result.Configuration = config;

            result.AppSetting = new AppSetting();
            config.Bind(result.AppSetting);

            return result;
        }

        public static void AddEnvironmentConfig(IConfigurationBuilder builder)
        {
            var appConfigTemp = builder.Build().Get<AppSetting>();

            builder.AddJsonFile(appConfigTemp.Configuration.ConfigFileKey, false, true);
        }


    }
}
