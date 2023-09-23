using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RespostaService : IRespostaService
    {
        private readonly IRespostaRepository _repo;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IDuvidaRepository _duvidaRepository;
        private readonly IMapper _map;
        public RespostaService(IRespostaRepository repo,
            IUsuarioRepository usuarioRepository,
            IDuvidaRepository duvidaRepository,
            IMapper map)
        {
            _repo = repo;
            _usuarioRepository = usuarioRepository;
            _duvidaRepository = duvidaRepository;
            _map = map;
        }
        public async Task AddAsync(RespostaDto resposta)
        {
            var respostaAdd = _map.Map<Resposta>(resposta);
            await _repo.AddAsync(respostaAdd);
        }

        public async Task<List<RespostaDto>> GetAllByDuvidaId(string duvidaId)
        {
            var result = await _repo.GetAllByDuvidaId(duvidaId);

            return _map.Map<List<RespostaDto>>(result);
        }

        public async Task UpdateAsync(RespostaDto resposta)
        {
            var respostaUpdate = _map.Map<Resposta>(resposta);
            await _repo.UpdateAsync(respostaUpdate);

            if(resposta.MelhorResposta.HasValue && resposta.MelhorResposta.Value)
            {
                var usuario = await _usuarioRepository.GetAsync(resposta.UsuarioId);

                var duvida = await _duvidaRepository.GetAsync(resposta.DuvidaId);

                usuario.Pontos = usuario.Pontos + duvida.Pontos;

                await _usuarioRepository.UpdatePontosAsync(usuario);
            }
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
