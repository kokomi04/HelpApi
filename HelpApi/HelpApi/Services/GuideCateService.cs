using AutoMapper;
using AutoMapper.QueryableExtensions;
using HelpApi.EF;
using HelpApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpApi.Services
{
    public interface IGuideCateService
    {
        Task<int> Create(GuideCateModel model);
        Task<bool> Delete(int guideCateId);
        Task<IList<GuideCateModel>> GetList();
        Task<GuideCateModel> Info(int guideCateId);
        Task<bool> Update(int guideCateId, GuideCateModel model);
    }

    public class GuideCateService : IGuideCateService
    {
        private readonly HelpDBContext _masterDBContext;

        private readonly IMapper _mapper;

        public GuideCateService(HelpDBContext masterDBContext, IMapper mapper)
        {
            _masterDBContext = masterDBContext;
            _mapper = mapper;
        }

        public async Task<int> Create(GuideCateModel model)
        {
            var entity = _mapper.Map<GuideCate>(model);

            _masterDBContext.GuideCate.Add(entity);
            await _masterDBContext.SaveChangesAsync();

            return entity.GuideCateId;
        }

        public async Task<bool> Delete(int guideCateId)
        {
            var g = await _masterDBContext.GuideCate.FirstOrDefaultAsync(g => g.GuideCateId == guideCateId);
            if (g == null)
                throw new Exception("Not found!");

            g.IsDeleted = true;
            await _masterDBContext.SaveChangesAsync();

            return true;
        }


        public async Task<GuideCateModel> Info(int guideCateId)
        {
            return await _masterDBContext.GuideCate.AsNoTracking()
                .ProjectTo<GuideCateModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(g => g.GuideCateId == guideCateId);
        }

        public async Task<IList<GuideCateModel>> GetList()
        {

            return await _masterDBContext.GuideCate.OrderBy(q => q.SortOrder)
                .ProjectTo<GuideCateModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

        }

        public async Task<bool> Update(int guideCateId, GuideCateModel model)
        {
            model.GuideCateId = guideCateId;
            if (model.ParentId == guideCateId)
            {
                throw new Exception("Bad Request!");
            }
            var g = await _masterDBContext.GuideCate.FirstOrDefaultAsync(g => g.GuideCateId == guideCateId);
            if (g == null)
                throw new Exception("Not found!");

            _mapper.Map(model, g);

            await _masterDBContext.SaveChangesAsync();

            return true;
        }
    }
}
