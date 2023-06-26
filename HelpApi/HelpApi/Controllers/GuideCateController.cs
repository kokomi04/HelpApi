using Applications.Models;
using Applications.Services;
using HelpApi.Controllers.BaseController;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpApi.Controllers
{
    [Route("api/guideCate")]
    public class GuideCateController : HelpBaseController
    {
        private readonly IGuideCateService _guideCateService;

        public GuideCateController(IGuideCateService guideCateService)
        {
            _guideCateService = guideCateService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IList<GuideCateModel>> GetList()
        {
            return await _guideCateService.GetList();
        }


        [HttpGet]
        [Route("{guideCateId}")]
        public async Task<GuideCateModel> Info([FromRoute] int guideCateId)
        {
            return await _guideCateService.Info(guideCateId);
        }

        [HttpPost]
        [Route("")]
        [Authorize(AuthenticationSchemes = "introspection")]
        public async Task<int> Create([FromBody] GuideCateModel model)
        {
            return await _guideCateService.Create(model);
        }

        [HttpPut]
        [Route("{guideCateId}")]
        [Authorize(AuthenticationSchemes = "introspection")]
        public async Task<bool> Update([FromRoute] int guideCateId, GuideCateModel model)
        {
            return await _guideCateService.Update(guideCateId, model);
        }

        [HttpDelete]
        [Route("{guideCateId}")]
        [Authorize(AuthenticationSchemes = "introspection")]
        public async Task<bool> Delete([FromRoute] int guideCateId)
        {
            return await _guideCateService.Delete(guideCateId);
        }
    }
}
