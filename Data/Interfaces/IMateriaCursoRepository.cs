using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMateriaCursoRepository
    {
        Task<List<string>> GetAllByCursoIdAsync(string cursoId);
    }
}
