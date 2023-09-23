using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAreaRepository
    {
        Task<List<Area>> GetAllAsync();
    }
}
