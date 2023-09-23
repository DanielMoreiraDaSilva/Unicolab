using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EventoService : IEventoService
    {
        private readonly IMapper _mapper;
        private readonly IEventoRepository _eventoRepo;

        public EventoService(IMapper mapper,
            IEventoRepository eventoRepo)
        {
            _mapper = mapper;
            _eventoRepo = eventoRepo;
        }

        public async Task<List<EventoDto>> GetAllAsync()
        {
            var result = await _eventoRepo.GetAllAsync();
            return _mapper.Map<List<EventoDto>>(result);
        }
        public async Task AddAsync(EventoDto evento)
        {
            await _eventoRepo.AddAsync(_mapper.Map<Evento>(evento));
        }
        public async Task UpdateAsync(EventoDto evento)
        {
            var eventoAtualizado = _mapper.Map<Evento>(evento);

            await _eventoRepo.UpdateAsync(eventoAtualizado);
        }

        public Task<List<Evento>> GetAllByCurso(string cursoId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ListaPaginada<Evento>> ListarEventosAsync(FiltroEvento filtro)
        {
            var result = await _eventoRepo.ListarEventosAsync(filtro);
            return result;
        }

        public async Task DeleteAsync(string id)
        {
            await _eventoRepo.DeleteAsync(id);
        }
    }
}