using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class DuvidaService : IDuvidaService
    {
        private readonly IDuvidaRepository _repo;
        private readonly IMateriaRepository _materiaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _map;
        public DuvidaService(IDuvidaRepository repo,
            IMateriaRepository materiaRepository,
            IUsuarioRepository usuarioRepository,
            IMapper map)
        {
            _repo = repo;
            _materiaRepository = materiaRepository;
            _usuarioRepository = usuarioRepository;
            _map = map;
        }

        public async Task<ListaPaginada<DuvidaDto>> ListarDuvidasAsync(FiltroDuvida filtro)
        {
            var usuario = await _usuarioRepository.GetAsync(filtro.UsuarioId);

            if(filtro.ListaMateria.Count == 0)
                filtro.ListaMateria = (await _materiaRepository.GetAllByUsuarioAsync(filtro.UsuarioId)).Select(x => x.Id).ToList();

            var result = await _repo.ListarDuvidasAsync(filtro);

            var resultDto = new ListaPaginada<DuvidaDto>()
            {
                Lista = _map.Map<List<DuvidaDto>>(result.Lista),
                Paginas = result.Paginas,
                TotalItens = result.TotalItens
            };

            return resultDto;
        }

        public async Task<string> AddAsync(DuvidaDto duvidaDto)
        {
            var duvida = _map.Map<Duvida>(duvidaDto);

            var result = await _repo.AddAsync(duvida);

            return result;
        }

        public async Task UpdateAsync(DuvidaDto duvidaDto)
        {
            var duvida = _map.Map<Duvida>(duvidaDto);
            await _repo.UpdateAsync(duvida);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
