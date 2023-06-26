using Applications.Models;
using AutoMapper;
using Infrastructures.EF.HelpDB;

namespace HelpApi.Utilities
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
