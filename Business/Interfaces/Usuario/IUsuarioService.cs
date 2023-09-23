using Business.TransferObjects;
using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> GetAsync(string Id);
        Task UpdateAsync(UsuarioDto usuario);
        Task AddAsync(UsuarioDto usuario);
        Task<ListaPaginada<Usuario>> ListarUsuariosAsync(FiltroUsuario filtro);
        Task<ListaPaginada<Usuario>> ListarUsuariosContatosAsync(FiltroUsuarioContato filtro);
        Task<bool> UsuarioExistenteAsync(string id, string nome);
        Task<bool> LoginExistenteAsync(string login);
        Task<bool> EmailExistenteAsync(string email);
        Task<UsuarioDto> LoginAsync(string login, string senha);
        Task<bool> AlterarSenhaAsync(string novaSenha, string token);
        Task<UsuarioDto> AlterarSenhaUsuarioAsync(string usuarioId, string novaSenha);
        Task<UsuarioDto> CadastrarUsuarioLoginAsync(UsuarioDto usuario);
        Task<UsuarioDto> ConsultarUsuarioPorLoginOuEmailAsync(string busca, string caminho);
        Task<List<UsuarioDto>> ListarUsuariosAtivosAsync();
    }
}
