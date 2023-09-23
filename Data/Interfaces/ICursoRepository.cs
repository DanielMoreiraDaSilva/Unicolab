using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICursoRepository
    {
        Task<List<Curso>> GetAllAsync(string usuarioId);
        Task<List<Curso>> GetByListIdAsync(List<string> cursoListaId);
    }
}
