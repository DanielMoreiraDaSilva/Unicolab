using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ILinkImportanteRepository
    {
        Task<List<LinkImportante>> GetAllAsync();
        Task<ListaPaginada<LinkImportante>> ListarLinksAsync(FiltroLinkImportante filtro);
        Task AddAsync(LinkImportante linkImportante);
        Task UpdateAsync(LinkImportante linkImportante);
        Task<bool> LinkExistenteAsync(string id, string link);
        Task<bool> TituloExistenteAsync(string id, string titulo);
    }
}
