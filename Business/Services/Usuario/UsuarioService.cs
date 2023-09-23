using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Constantes;
using Data.Interfaces;
using Data.Models;
using Data.Models.Filtros;
using Data.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IModuloRepository _moduloRepository;
        private readonly IMateriaCursoRepository _materiaCursoRepository;
        private readonly IPerfilService _perfilService;
        private readonly IUsuarioMateriaRepository _usuarioMateriaRepository;
        private readonly IUsuarioCursoRepository _usuarioCursoRepository;
        private readonly ICursoService _cursoService;
        public UsuarioService(IMapper mapper,
            IUsuarioRepository usuarioRepo,
            IModuloRepository moduloRepository,
            IMateriaCursoRepository materiaCursoRepository,
            IPerfilService perfilService,
            IUsuarioMateriaRepository usuarioMateriaRepository,
            IUsuarioCursoRepository usuarioCursoRepository,
            ICursoService cursoService)
        {
            _mapper = mapper;
            _usuarioRepo = usuarioRepo;
            _moduloRepository = moduloRepository;
            _materiaCursoRepository = materiaCursoRepository;
            _perfilService = perfilService;
            _usuarioMateriaRepository = usuarioMateriaRepository;
            _usuarioCursoRepository = usuarioCursoRepository;
            _cursoService = cursoService;
        }

        public async Task AddAsync(UsuarioDto usuario)
        {
            var usuarioAdd = _mapper.Map<Usuario>(usuario);

            if (usuario.PerfilId == Resources.PERFIL_ALUNO)
                usuarioAdd.Pontos = 0;

            var id = await _usuarioRepo.AddAsync(usuarioAdd);

            if (usuario.ListaCursoId.Count > 0)
            {
                foreach (var cursoId in usuario.ListaCursoId)
                {
                    await _usuarioCursoRepository.AddAsync(new UsuarioCurso() { UsuarioId = id, CursoId = cursoId });

                    var materiasId = await _materiaCursoRepository.GetAllByCursoIdAsync(cursoId);

                    foreach (var materiaId in materiasId)
                    {
                        await _usuarioMateriaRepository.AddAsync(new UsuarioMateria() { UsuarioId = id, MateriaId = materiaId });
                    }
                }
            }
        }

        public async Task<UsuarioDto> GetAsync(string Id)
        {
            var result = _mapper.Map<UsuarioDto>(await _usuarioRepo.GetAsync(Id));


            if (result != null)
            {
                result.Perfil = await _perfilService.GetAsync(result.PerfilId);

                result.Perfil.Modulos = _mapper.Map<List<ModuloDto>>(await _moduloRepository.GetAllByPerfilIdAsync(result.Perfil.Id));

                var materiaIds = await _usuarioMateriaRepository.GetAllByUsuarioIdAsync(Id);

                result.MateriaIds = materiaIds.Select(x => x.MateriaId).ToList();

                var cursoIds = await _usuarioCursoRepository.GetAllByUsuarioIdAsync(Id);

                result.ListaCursoId = cursoIds.Select(x => x.CursoId).ToList();
                result.Cursos = await _cursoService.GetByListIdAsync(cursoIds.Select(x => x.CursoId).ToList()); 
            }

            return result;
        }

        public async Task UpdateAsync(UsuarioDto usuario)
        {
            if(usuario.ListaCursoId.Count > 0)
            {
                await _usuarioCursoRepository.DeleteByUsuarioId(usuario.Id);

                foreach (var cursoId in usuario.ListaCursoId)
                {
                    await _usuarioCursoRepository.AddAsync(new UsuarioCurso() { UsuarioId = usuario.Id, CursoId = cursoId });

                    var materiasId = await _materiaCursoRepository.GetAllByCursoIdAsync(cursoId);

                    await _usuarioMateriaRepository.DeleteAsync(usuario.Id);

                    foreach (var materiaId in materiasId)
                    {
                        await _usuarioMateriaRepository.AddAsync(new UsuarioMateria() { UsuarioId = usuario.Id, MateriaId = materiaId });
                    }
                }
            }
            var usuarioUpdate = _mapper.Map<Usuario>(usuario);
            await _usuarioRepo.UpdateAsync(usuarioUpdate);
        }

        public async Task<ListaPaginada<Usuario>> ListarUsuariosAsync(FiltroUsuario filtro)
        {
            var result = await _usuarioRepo.ListarUsuariosAsync(filtro);

            foreach (var usuario in result.Lista)
            {
                // Buscar Perfis
            }

            return result;
        }

        public async Task<bool> UsuarioExistenteAsync(string id, string nome)
        {
            return await _usuarioRepo.UsuarioExistenteAsync(id, nome);
        }

        public async Task<bool> LoginExistenteAsync(string login)
        {
            return await _usuarioRepo.LoginExistenteAsync(login);
        }

        public async Task<bool> EmailExistenteAsync(string email)
        {
            return await _usuarioRepo.EmailExistenteAsync(email);
        }

        public async Task<UsuarioDto> LoginAsync(string login, string senha)
        {
            var usuario = await _usuarioRepo.LoginAsync(login, senha);
            if (usuario != null)
            {
                usuario.Token = GeradorToken.GerarToken();
                usuario.DataUltimoToken = DateTime.Now;
                await _usuarioRepo.AtualizarTokenAsync(usuario);
                var usuarioDto = await GetAsync(usuario.Id);
                return usuarioDto;
            }
            else
                return null;
        }

        public async Task<bool> AlterarSenhaAsync(string novaSenha, string token)
        {
            return await _usuarioRepo.AlterarSenhaAsync(novaSenha, token);
        }

        public async Task<UsuarioDto> AlterarSenhaUsuarioAsync(string usuarioId, string novaSenha)
        {
            return _mapper.Map<UsuarioDto>(await _usuarioRepo.AlterarSenhaUsuarioWebAsync(usuarioId, novaSenha));
        }

        public async Task<UsuarioDto> CadastrarUsuarioLoginAsync(UsuarioDto usuario)
        {
            var usuarioAdd = _mapper.Map<Usuario>(usuario);
            var usuarioId = (await _usuarioRepo.CadastrarUsuarioLoginAsync(usuarioAdd)).Id;
            return await GetAsync(usuarioId);
        }

        public async Task<UsuarioDto> ConsultarUsuarioPorLoginOuEmailAsync(string busca, string caminho)
        {
            var usuarioId = (await _usuarioRepo.ConsultarUsuarioPorLoginOuEmailAsync(busca)).Id;
            var usuario = await GetAsync(usuarioId);
            //if (usuario != null)
            //{
            //    var body = new TemplateEmailResetSenhaDto()
            //    {
            //        Link = _baseAddress.UriServimed + "?id=" + usuario.Id.ToString(),
            //        Caminho = Path.GetFullPath("Templates/EmailResetSenha.html")
            //    };

            //    MailManager _mailManager = new MailManager(_logEmail);
            //    _mailManager.ConfigSmtpEmail(_smtpEmailConfig);
            //    _mailManager.SendMail(new string[1] { usuario.Email }, Resources.TITULO_EMAIL_RESET_SENHA, body.Template(), null);
            //}
            return usuario;
        }

        public async Task<List<UsuarioDto>> ListarUsuariosAtivosAsync()
        {
            return _mapper.Map<List<UsuarioDto>>(await _usuarioRepo.ListarUsuariosAtivosAsync());
        }

        public async Task<ListaPaginada<Usuario>> ListarUsuariosContatosAsync(FiltroUsuarioContato filtro)
        {
            var filtroMap = _mapper.Map<FiltroUsuario>(filtro);

            var result = await _usuarioRepo.ListarUsuariosAsync(filtroMap);

            foreach (var usuario in result.Lista)
            {
                // Buscar Perfis
            }

            return result;
        }
    }
}
