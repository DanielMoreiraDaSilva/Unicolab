using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMateriaRepository
    {
        Task<List<Materia>> GetAllAsync();
        Task<List<Materia>> GetAllByUsuarioAsync(string usuarioId);
    }
}
