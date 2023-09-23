using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AreaService : IAreaService
    {
        private readonly IMapper _mapper;
        private readonly IAreaRepository _areaRepo;

        public AreaService(IMapper mapper,
            IAreaRepository areaRepo)
        {
            _mapper = mapper;
            _areaRepo = areaRepo;
        }

        public async Task<List<AreaDto>> GetAllAsync()
        {
            var result = await _areaRepo.GetAllAsync();
            return _mapper.Map<List<AreaDto>>(result);
        }

    }
}