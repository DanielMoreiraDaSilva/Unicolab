using Business.TransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRespostaService
    {
        Task<List<RespostaDto>> GetAllByDuvidaId(string duvidaId);
        Task AddAsync(RespostaDto resposta);
        Task UpdateAsync(RespostaDto resposta);
        Task DeleteAsync(string id);
    }
}
