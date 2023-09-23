using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IOportunidadeRepository
    {
        Task<List<Oportunidade>> GetAllAsync();
        Task<List<Oportunidade>> GetAllByCurso(string cursoId);
        Task<ListaPaginada<Oportunidade>> ListarOportunidadesAsync(FiltroOportunidade filtro);
        Task UpdateAsync(Oportunidade oportunidade);
        Task AddAsync(Oportunidade oportunidade);
        Task DeleteAsync(string id);
    }
}