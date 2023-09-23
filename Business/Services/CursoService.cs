using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CursoService : ICursoService
    {
        private readonly IMapper _mapper;
        private readonly ICursoRepository _cursoRepo;

        public CursoService(IMapper mapper,
            ICursoRepository cursoRepo)
        {
            _mapper = mapper;
            _cursoRepo = cursoRepo;
        }

        public async Task<List<CursoDto>> GetAllAsync(string usuarioId)
        {
            var result = await _cursoRepo.GetAllAsync(usuarioId);
            return _mapper.Map<List<CursoDto>>(result);
        }

        public async Task<List<CursoDto>> GetByListIdAsync(List<string> cursoListaId)
        {
            var result = await _cursoRepo.GetByListIdAsync(cursoListaId);
            return _mapper.Map<List<CursoDto>>(result);
        }
    }
}
