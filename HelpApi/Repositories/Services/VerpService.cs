using Infrastructures.AppConfigs.Model;
using Infrastructures.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services
{
    public interface IVerpService
    {
        string GenerateHelpToken();
    }
    public class VerpService : IVerpService
    {
        private readonly AppSetting _appSetting;

        public VerpService(IOptionsSnapshot<AppSetting> appSetting) 
        {
            _appSetting = appSetting?.Value;
        }
        public string GenerateHelpToken()
        {
            return JwtUtils.GenerateJwtToken(_appSetting);
        }

        
    }
}
