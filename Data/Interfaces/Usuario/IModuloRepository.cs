using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IModuloRepository
    {
        Task<List<Modulo>> GetAllAsync();
        Task<List<Modulo>> GetAllByPerfilIdAsync(string perfilId);
    }
}
