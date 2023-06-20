﻿using HelpApi.Controllers.BaseController;
using HelpApi.Models;
using HelpApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VErpApi.Controllers.System
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
        public async Task<int> Create([FromBody] GuideCateModel model)
        {
            return await _guideCateService.Create(model);
        }

        [HttpPut]
        [Route("{guideCateId}")]
        public async Task<bool> Update([FromRoute] int guideCateId, GuideCateModel model)
        {
            return await _guideCateService.Update(guideCateId, model);
        }

        [HttpDelete]
        [Route("{guideCateId}")]
        public async Task<bool> Delete([FromRoute] int guideCateId)
        {
            return await _guideCateService.Delete(guideCateId);
        }
    }
}
