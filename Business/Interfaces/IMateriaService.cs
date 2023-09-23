using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMateriaService
    {
        Task<List<MateriaDto>> GetAllAsync();
        Task<List<MateriaDto>> GetAllByUsuarioAsync(string usuarioId);
    }
}
