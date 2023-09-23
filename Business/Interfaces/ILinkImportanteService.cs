using Business.TransferObjects;
using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ILinkImportanteService
    {
        Task<List<LinkImportanteDto>> GetAllAsync();
        Task<ListaPaginada<LinkImportante>> ListarLinksAsync(FiltroLinkImportante filtro);
        Task AddAsync(LinkImportanteDto linkImportante);
        Task UpdateAsync(LinkImportanteDto linkImportante);
        Task<bool> LinkExistenteAsync(string id, string link);
        Task<bool> TituloExistenteAsync(string id, string titulo);
    }
}