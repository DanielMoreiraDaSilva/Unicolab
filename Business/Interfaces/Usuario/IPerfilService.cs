using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPerfilService
    {
        Task<List<PerfilDto>> GetAllAsync();
        Task<PerfilDto> GetAsync(string id);
    }
}
