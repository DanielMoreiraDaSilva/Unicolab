using AutoMapper;
using Business.TransferObjects;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UsuarioMateriaService : IUsuarioMateriaService
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioMateriaRepository _usuarioMateriaRepo;

        public UsuarioMateriaService(IMapper mapper,
            IUsuarioMateriaRepository usuarioMateriaRepo)
        {
            _mapper = mapper;
            _usuarioMateriaRepo = usuarioMateriaRepo;
        }

        public async Task<List<UsuarioMateriaDto>> GetAllAsync()
        {
            var result = await _usuarioMateriaRepo.GetAllAsync();
            return _mapper.Map<List<UsuarioMateriaDto>>(result);
        }

        public async Task<List<UsuarioMateriaDto>> GetAllByUsuarioIdAsync(string usuarioId)
        {
            var result = await _usuarioMateriaRepo.GetAllByUsuarioIdAsync(usuarioId);
            return _mapper.Map<List<UsuarioMateriaDto>>(result);
        }
    }
}