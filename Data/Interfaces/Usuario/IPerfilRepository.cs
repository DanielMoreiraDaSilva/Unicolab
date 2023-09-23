using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPerfilRepository
    {
        Task<List<Perfil>> GetAllAsync();
        Task<Perfil> GetAsync(string id);
    }
}
