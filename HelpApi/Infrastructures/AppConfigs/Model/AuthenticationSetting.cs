using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.AppConfigs.Model
{
    public class AuthenticationSetting
    {
        public string ValidAudience { get; set; }

        public string ValidIssuer { get; set; }

        public string SecretKey { get; set; }
    }
}
