using Data.Models;
using Data.Models.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<string> AddAsync(Usuario usuario);
        Task<Usuario> CadastrarUsuarioLoginAsync(Usuario usuario);
        Task<Usuario> GetAsync(string Id);
        Task UpdateAsync(Usuario usuarioUpdate);
        Task<ListaPaginada<Usuario>> ListarUsuariosAsync(FiltroUsuario filtro, bool paginarRegistros = true);
        Task<bool> UsuarioExistenteAsync(string id, string nome);
        Task<bool> LoginExistenteAsync(string login);
        Task<bool> EmailExistenteAsync(string email);
        Task<Usuario> LoginAsync(string login, string senha);
        Task<bool> AlterarSenhaAsync(string novaSenha, string token);
        Task<Usuario> AlterarSenhaUsuarioWebAsync(string usuarioId, string novaSenha);
        Task<Usuario> AtualizarTokenAsync(Usuario usuario);
        Task<bool> UsuarioAutorizadoAsync(string token);
        Task<Usuario> AlterarSenhaUsuarioAsync(string usuarioId, string novaSenha);
        Task<Usuario> ConsultarUsuarioPorLoginOuEmailAsync(string busca);
        Task<List<Usuario>> ListarUsuariosAtivosAsync();
        Task<Usuario> GetByCodigoAsync(int codigo);
        Task UpdatePontosAsync(Usuario usuarioUpdate);
    }
}
