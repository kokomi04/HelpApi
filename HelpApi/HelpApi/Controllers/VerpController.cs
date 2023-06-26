using Applications.Services;
using HelpApi.Controllers.BaseController;
using Infrastructures.AppConfigs.Model;
using Infrastructures.AppConfigs.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;

namespace HelpApi.Controllers
{
    [Route("api/verp")]
    public class VerpController : ControllerBase
    {
        private readonly IVerpService _verpService;
        private readonly AppSetting _appSetting;

        public VerpController(IVerpService verpService, IOptionsSnapshot<AppSetting> appSetting)
        {
            _verpService = verpService;
            _appSetting = appSetting?.Value;
        }

        // GET: VerpController
        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            var secretKey = Request.Headers["SecretKey"];

            if (!IsValidSecretKey(secretKey))
            {
                return Unauthorized();
            }

            var token = _verpService.GenerateHelpToken();

            HttpContext.Response.Headers.Add("Token", token);

            return Ok();
        }

        private bool IsValidSecretKey(string secretKey)
        {
            return secretKey == _appSetting.ExternalHelpApiKey;
        }
    }
}
