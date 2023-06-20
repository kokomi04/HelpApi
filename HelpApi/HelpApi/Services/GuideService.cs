

using AutoMapper;
using AutoMapper.QueryableExtensions;
using HelpApi.Commons.GlobalObject;
using HelpApi.EF;
using HelpApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HelpApi.Services
{
    public interface IGuideService
    {
        //Task<PageData<GuideModelOutput>> GetList(string keyword, int? guideCateId, int page, int size);
        Task<GuideModel> GetGuideById(int guideId);
        Task<IList<GuideModel>> GetGuidesByCode(string guideCode);
        Task<bool> Update(int guideId, GuideModel model);
        Task<int> Create(GuideModel model);
        Task<bool> Deleted(int guideId);
    }

    public class GuideService : IGuideService
    {
        private readonly MasterDBContext _masterDBContext;

        private readonly IMapper _mapper;

        public GuideService(MasterDBContext masterDBContext, IMapper mapper)
        {
            _masterDBContext = masterDBContext;
            _mapper = mapper;
        }

        public async Task<int> Create(GuideModel model)
        {
            var entity = _mapper.Map<Guide>(model);

            _masterDBContext.Guide.Add(entity);
            await _masterDBContext.SaveChangesAsync();

            return entity.GuideId;
        }

        public async Task<bool> Deleted(int guideId)
        {
            var g = await _masterDBContext.Guide.FirstOrDefaultAsync(g => g.GuideId == guideId);
            if (g == null)
                throw new Exception("Not found!");

            g.IsDeleted = true;
            await _masterDBContext.SaveChangesAsync();

            return true;
        }

        public async Task<IList<GuideModel>> GetGuidesByCode(string guideCode)
        {
            return await _masterDBContext.Guide.AsNoTracking()
                .Where(g => g.GuideCode.Equals(guideCode))
                .OrderBy(x => x.SortOrder)
                .ProjectTo<GuideModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<GuideModel> GetGuideById(int guideId)
        {
            return await _masterDBContext.Guide.AsNoTracking()
                .ProjectTo<GuideModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(g => g.GuideId == guideId);
        }

        public async Task<bool> Update(int guideId, GuideModel model)
        {
            var g = await _masterDBContext.Guide.FirstOrDefaultAsync(g => g.GuideId == guideId);
            if (g == null)
                throw new Exception("Not found!");

            _mapper.Map(model, g);

            await _masterDBContext.SaveChangesAsync();

            return true;
        }
    }
}
