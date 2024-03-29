﻿using System;

namespace Infrastructures.AppConfigs
{
    public static class EnviromentConfig
    {
        public static bool IsUnitTest
        {
            get
            {
                return Environment.GetEnvironmentVariable("DEBUG_ENVIRONMENT") == "UNIT_TEST";
            }
        }

        public static string EnviromentName
        {
            get
            {
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            }
        }

        public static bool IsProduction
        {
            get
            {
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            }
        }
    }
}
