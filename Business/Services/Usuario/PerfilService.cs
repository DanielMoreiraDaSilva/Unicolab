using AutoMapper;
using Business.TransferObjects;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IMapper _mapper;
        private readonly IPerfilRepository _perfilRepo;
        public PerfilService(IMapper mapper,
            IPerfilRepository perfilRepo)
        {
            _mapper = mapper;
            _perfilRepo = perfilRepo;
        }

        public async Task<List<PerfilDto>> GetAllAsync()
        {
            var result = await _perfilRepo.GetAllAsync();     
            return _mapper.Map<List<PerfilDto>>(result);
        }

        public async Task<PerfilDto> GetAsync(string id)
        {
            var perfil = await _perfilRepo.GetAsync(id);

            var perfilDto = _mapper.Map<PerfilDto>(perfil);

            return perfilDto;
        }
    }
}
