using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUsuarioMateriaRepository
    {
        Task<List<UsuarioMateria>> GetAllAsync();
        Task<List<UsuarioMateria>> GetAllByUsuarioIdAsync(string usuarioId);
        Task AddAsync(UsuarioMateria usuarioMateria);
        Task DeleteAsync(string usuarioId);
    }
}