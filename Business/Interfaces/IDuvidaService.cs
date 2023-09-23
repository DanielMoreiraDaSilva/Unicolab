using Business.TransferObjects;
using Data.Models;
using Data.Models.Filtros;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IDuvidaService
    {
        Task<ListaPaginada<DuvidaDto>> ListarDuvidasAsync(FiltroDuvida filtro);
        Task<string> AddAsync(DuvidaDto duvidaDto);
        Task UpdateAsync(DuvidaDto duvidaDto);
        Task DeleteAsync(string id);
    }
}
