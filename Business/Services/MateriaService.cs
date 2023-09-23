using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MateriaService : IMateriaService
    {
        private readonly IMapper _mapper;
        private readonly IMateriaRepository _materiaRepo;

        public MateriaService(IMapper mapper,
            IMateriaRepository cursoRepo)
        {
            _mapper = mapper;
            _materiaRepo = cursoRepo;
        }

        public async Task<List<MateriaDto>> GetAllAsync()
        {
            var result = await _materiaRepo.GetAllAsync();
            return _mapper.Map<List<MateriaDto>>(result);
        }

        public async Task<List<MateriaDto>> GetAllByUsuarioAsync(string usuarioId)
        {
            var result = await _materiaRepo.GetAllByUsuarioAsync(usuarioId);
            return _mapper.Map<List<MateriaDto>>(result);
        }
    }
}
