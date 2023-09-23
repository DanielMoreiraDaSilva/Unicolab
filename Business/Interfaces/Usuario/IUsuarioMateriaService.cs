using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUsuarioMateriaService
    {
        Task<List<UsuarioMateriaDto>> GetAllAsync();
        Task<List<UsuarioMateriaDto>> GetAllByUsuarioIdAsync(string usuarioId);
    }
}