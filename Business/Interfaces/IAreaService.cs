using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAreaService
    {
        Task<List<AreaDto>> GetAllAsync();
    }
}