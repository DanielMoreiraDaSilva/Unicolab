using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class OportunidadeCursoService : IOportunidadeCursoService
    {
        private readonly IMapper _mapper;
        private readonly IOportunidadeCursoRepository _oportunidadecursoRepo;

        public OportunidadeCursoService(IMapper mapper,
            IOportunidadeCursoRepository oportucursRepo)
        {
            _mapper = mapper;
            _oportunidadecursoRepo = oportucursRepo;
        }

        public async Task<List<OportunidadeCursoDto>> GetAllAsync()
        {
            var result = await _oportunidadecursoRepo.GetAllAsync();
            return _mapper.Map<List<OportunidadeCursoDto>>(result);
        }
    }
}
