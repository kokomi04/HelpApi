using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpApi.Controllers.BaseController
{
    [ApiController]
    [Authorize]
    public abstract class HelpBaseController : ControllerBase
    {
        
    }
}