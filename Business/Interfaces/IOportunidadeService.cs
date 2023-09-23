using Business.TransferObjects;
using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IOportunidadeService
    {
        Task<List<OportunidadeDto>> GetAllAsync();
        Task<List<OportunidadeDto>> GetAllByCurso(string cursoId);
        Task<ListaPaginada<Oportunidade>> ListarOportunidadesAsync(FiltroOportunidade filtro);
        Task UpdateAsync(OportunidadeDto oportunidade);
        Task AddAsync(OportunidadeDto oportunidade);
        Task DeleteAsync(string id);
    }
}
