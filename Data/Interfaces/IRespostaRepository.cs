using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRespostaRepository
    {
        Task<List<Resposta>> GetAllByDuvidaId(string duvidaId);
        Task AddAsync(Resposta resposta);
        Task UpdateAsync(Resposta resposta);
        Task DeleteAsync(string id);
    }
}
