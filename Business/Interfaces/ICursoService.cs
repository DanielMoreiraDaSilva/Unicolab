using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICursoService
    {
        Task<List<CursoDto>> GetAllAsync(string usuarioId);
        Task<List<CursoDto>> GetByListIdAsync(List<string> cursoListaId);
    }
}
