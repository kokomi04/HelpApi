using Applications.Models;
using Applications.Services;
using HelpApi.Controllers.BaseController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpApi.Controllers
{
    [Route("api/guides")]
    public class GuideController : HelpBaseController
    {
        private readonly IGuideService _guideService;

        public GuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        [HttpGet]
        [Route("byCode/{guideCode}")]
        public async Task<IList<GuideModel>> GetListGuideByCode([FromRoute] string guideCode)
        {
            return await _guideService.GetGuidesByCode(guideCode);
        }
        [HttpGet]
        [Route("{guideId}")]
        public async Task<GuideModel> GetGuideById([FromRoute] int guideId)
        {
            return await _guideService.GetGuideById(guideId);
        }

        [HttpPost]
        [Route("")]
        public async Task<int> Create([FromBody] GuideModel model)
        {
            return await _guideService.Create(model);
        }

        [HttpPut]
        [Route("{guideId}")]
        public async Task<bool> Update([FromRoute] int guideId, GuideModel model)
        {
            return await _guideService.Update(guideId, model);
        }

        [HttpDelete]
        [Route("{guideId}")]
        public async Task<bool> Deleted([FromRoute] int guideId)
        {
            return await _guideService.Deleted(guideId);
        }
    }
}
