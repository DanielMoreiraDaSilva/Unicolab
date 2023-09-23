using AutoMapper;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class LinkImportanteService : ILinkImportanteService
    {
        private readonly IMapper _mapper;
        private readonly ILinkImportanteRepository _linkImportanteRepo;
		
        public LinkImportanteService(IMapper mapper,
            ILinkImportanteRepository linkImportanteRepo)
        {
            _mapper = mapper;
            _linkImportanteRepo = linkImportanteRepo;
        }

        public async Task AddAsync(LinkImportanteDto linkImportante)
        {
            await _linkImportanteRepo.AddAsync(_mapper.Map<LinkImportante>(linkImportante));
        }

        public async Task<List<LinkImportanteDto>> GetAllAsync()
        {
            var result = await _linkImportanteRepo.GetAllAsync();     
            return _mapper.Map<List<LinkImportanteDto>>(result);
        }

        public async Task<bool> LinkExistenteAsync(string id, string link)
        {
            return await _linkImportanteRepo.LinkExistenteAsync(id, link);
        }

        public async Task<ListaPaginada<LinkImportante>> ListarLinksAsync(FiltroLinkImportante filtro)
        {
            var result = await _linkImportanteRepo.ListarLinksAsync(filtro);
            return result;
        }

        public async Task<bool> TituloExistenteAsync(string id, string titulo)
        {
            return await _linkImportanteRepo.TituloExistenteAsync(id, titulo);
        }

        public async Task UpdateAsync(LinkImportanteDto linkImportante)
        {
            var linkImportanteAtualizado = _mapper.Map<LinkImportante>(linkImportante);

            await _linkImportanteRepo.UpdateAsync(linkImportanteAtualizado);
        }
    }
}
