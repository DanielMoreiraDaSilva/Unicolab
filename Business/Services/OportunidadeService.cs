using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models.Filtros;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class OportunidadeService : IOportunidadeService
    {
        private readonly IMapper _mapper;
        private readonly IOportunidadeRepository _oportunidadeRepo;

        public OportunidadeService(IMapper mapper,
            IOportunidadeRepository oportunidadeRepo)
        {
            _mapper = mapper;
            _oportunidadeRepo = oportunidadeRepo;
        }

        public async Task<List<OportunidadeDto>> GetAllAsync()
        {
            var result = await _oportunidadeRepo.GetAllAsync();
            return _mapper.Map<List<OportunidadeDto>>(result);
        }
        public async Task<List<OportunidadeDto>> GetAllByCurso(string cursoId)
        {
            var result = _mapper.Map<List<OportunidadeDto>>(await _oportunidadeRepo.GetAllByCurso(cursoId));

            return result;
        }

        public async Task<ListaPaginada<Oportunidade>> ListarOportunidadesAsync(FiltroOportunidade filtro)
        {
            var result = await _oportunidadeRepo.ListarOportunidadesAsync(filtro);
            return result;
        }

        public async Task UpdateAsync(OportunidadeDto oportunidade)
        {
            var oportunidadeAtualizada = _mapper.Map<Oportunidade>(oportunidade);

            await _oportunidadeRepo.UpdateAsync(oportunidadeAtualizada);
        }
        public async Task AddAsync(OportunidadeDto oportunidade)
        {
            await _oportunidadeRepo.AddAsync(_mapper.Map<Oportunidade>(oportunidade));
        }

        public async Task DeleteAsync(string id)
        {
            await _oportunidadeRepo.DeleteAsync(id);
        }
    }
}
