using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUsuarioCursoRepository
    {
        public Task<List<UsuarioCurso>> GetAllAsync();
        public Task<List<UsuarioCurso>> GetAllByUsuarioIdAsync(string usuarioId);
        Task AddAsync(UsuarioCurso usuarioCurso);
        Task DeleteByUsuarioId(string usuarioId);
    }
}
