using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IEventoRepository
    {
        Task<List<Evento>> GetAllAsync();
        Task AddAsync(Evento evento);
        Task UpdateAsync(Evento evento);
        Task<ListaPaginada<Evento>> ListarEventosAsync(FiltroEvento filtro);
        Task DeleteAsync(string id);
    }
}
