using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDuvidaRepository
    {
        Task<ListaPaginada<Duvida>> ListarDuvidasAsync(FiltroDuvida filtro);
        Task<string> AddAsync(Duvida duvida);
        Task UpdateAsync(Duvida duvida);
        Task<Duvida> GetAsync(string id);
        Task DeleteAsync(string id);
    }
}
