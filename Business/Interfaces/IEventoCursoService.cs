using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEventoCursoService
    {
        Task<List<EventoCursoDto>> GetAllAsync();
    }
}
