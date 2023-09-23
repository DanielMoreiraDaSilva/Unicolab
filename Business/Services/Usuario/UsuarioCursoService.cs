using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UsuarioCursoService : IUsuarioCursoService
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioCursoRepository _usuarioCursoRepo;

        public UsuarioCursoService(IMapper mapper,
            IUsuarioCursoRepository usuarioCursoRepo)
        {
            _mapper = mapper;
            _usuarioCursoRepo = usuarioCursoRepo;
        }
        public async Task<List<UsuarioCursoDto>> GetAllAsync()
        {
            var result = await _usuarioCursoRepo.GetAllAsync();
            return _mapper.Map<List<UsuarioCursoDto>>(result);
        }

        public async Task<List<UsuarioCursoDto>> GetAllByUsuarioIdAsync(string usuarioId)
        {
            var result = await _usuarioCursoRepo.GetAllByUsuarioIdAsync(usuarioId);
            return _mapper.Map<List<UsuarioCursoDto>>(result);
        }

        public async Task AddAsync(UsuarioCursoDto usuarioCurso)
        {
            await _usuarioCursoRepo.AddAsync(_mapper.Map<UsuarioCurso>(usuarioCurso));
        }

        public async Task DeleteByUsuarioId(string usuarioId)
        {
            await _usuarioCursoRepo.DeleteByUsuarioId(usuarioId);
        }
    }
}
