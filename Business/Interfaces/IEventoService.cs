using Business.TransferObjects;
using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEventoService
    {
        Task<List<EventoDto>> GetAllAsync();
        Task AddAsync(EventoDto evento);
        Task UpdateAsync(EventoDto evento);
        Task<List<Evento>> GetAllByCurso(string cursoId);
        Task<ListaPaginada<Evento>> ListarEventosAsync(FiltroEvento filtro);
        Task DeleteAsync(string id);
    }
}