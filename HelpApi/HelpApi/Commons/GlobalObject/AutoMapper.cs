using AutoMapper;
using HelpApi.EF;
using HelpApi.Models;
using Microsoft.Extensions.Logging;

namespace HelpApi.Commons.GlobalObject
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Guide, GuideModel>().ReverseMap();
            CreateMap<GuideCate, GuideCateModel>().ReverseMap();
        }
    }
}
