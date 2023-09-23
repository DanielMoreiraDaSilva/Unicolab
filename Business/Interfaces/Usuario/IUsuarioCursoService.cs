using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUsuarioCursoService
    {
        Task<List<UsuarioCursoDto>> GetAllAsync();
        Task<List<UsuarioCursoDto>> GetAllByUsuarioIdAsync(string usuarioId);
        Task AddAsync(UsuarioCursoDto usuarioCurso);
        Task DeleteByUsuarioId(string usuarioId);
    }
}
