using Dapper;
using Data.Interfaces;
using Data.Models;
using Data.Models.Enums;
using Data.Models.Filtros;
using Data.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Func<IDbConnection> _connection;
        public UsuarioRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }

        public async Task<string> AddAsync(Usuario usuario)
        {
            var query = @"INSERT INTO USUARIO 
                            VALUES(
                            @ID,
                            @NOME,
                            @SENHA,
                            @EMAIL,
                            @LOGIN,
                            @ATIVO,
                            @PRIMEIROACESSO,
                            @ULTIMOACESSO,
                            @TOKEN,
                            @DATAULTIMOTOKEN,
                            @PERFILID,
                            @RESETSOLICITADO,
                            @PONTOS,
                            @RA
                            )";

            var parametros = new DynamicParameters();
            var id = Guid.NewGuid().ToString().ToUpper();
            parametros.Add("@ID", id);
            parametros.Add("@NOME", usuario.Nome.Trim());
            parametros.Add("@SENHA", CriptografiaSenha.Criptografar(usuario.Senha));
            parametros.Add("@EMAIL", usuario.Email.Trim());
            parametros.Add("@LOGIN", usuario.Login.Trim());
            parametros.Add("@ATIVO", 1);
            parametros.Add("@PRIMEIROACESSO", 1);
            parametros.Add("@ULTIMOACESSO", DateTime.Now);
            parametros.Add("@TOKEN", GeradorToken.GerarToken());
            parametros.Add("@DATAULTIMOTOKEN", DateTime.Now);
            parametros.Add("@PERFILID", usuario.PerfilId);
            parametros.Add("@RESETSOLICITADO", 0);
            parametros.Add("@PONTOS", 0);
            parametros.Add("@RA", usuario.RA);
            //AddDynamicParameters

            using (IDbConnection connection = _connection.Invoke())
            {
                try
                {
                    await connection.ExecuteAsync(query, parametros);

                }
                catch (Exception ex)
                {
                    string erro = ex.Message;
                }
                return id;
            }
        }

        public async Task<Usuario> CadastrarUsuarioLoginAsync(Usuario usuario)
        {
            var senha = usuario.Senha;
            usuario.PrimeiroAcesso = false;
            usuario.UltimoAcesso = DateTime.Now;
            usuario.DataUltimoToken = DateTime.Now;
            usuario.Token = GeradorToken.GerarToken();
            usuario.Senha = CriptografiaSenha.Criptografar(usuario.Senha);
            usuario.Ativo = true;
            usuario.PerfilId = "";

            await AddAsync(usuario);

            return await LoginAsync(usuario.Login, senha);
        }

        public async Task<Usuario> GetAsync(string Id)
        {

            var query = @"SELECT U.*, A.DESCRICAO AREADESCRICAO, C.DESCRICAO CURSODESCRICAO
                            FROM USUARIO U
                            INNER JOIN USUARIOCURSO UC
                            ON UC.USUARIOID = U.ID
                            INNER JOIN CURSO C
                            ON C.ID = UC.CURSOID
                            INNER JOIN AREA A
                            ON A.ID = C.AREAID
                            WHERE U.ID = @ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", Id);

            using (IDbConnection connection = _connection.Invoke())
            {
                var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                return usuario;
            }
        }

        public async Task UpdatePontosAsync(Usuario usuarioUpdate)
        {
            var query = @"UPDATE USUARIO
                            SET 
                            PONTOS=@PONTOS
                            WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", usuarioUpdate.Id);
            parametros.Add("@PONTOS", usuarioUpdate.Pontos);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }

        public async Task UpdateAsync(Usuario usuarioUpdate)
        {

            var usuarioBase = await GetAsync(usuarioUpdate.Id);

            var query = @"UPDATE USUARIO
                            SET 
                            NOME=@NOME, 
                            EMAIL=@EMAIL, 
                            LOGIN=@LOGIN, 
                            ATIVO=@ATIVO, 
                            SENHA=@SENHA, 
                            TOKEN=@TOKEN, 
                            PRIMEIROACESSO=@PRIMEIROACESSO,
                            ULTIMOACESSO=@ULTIMOACESSO,
                            DATAULTIMOTOKEN=@DATAULTIMOTOKEN, 
                            PERFILID=@PERFILID, 
                            RESETSOLICITADO=@RESETSOLICITADO
                            WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", usuarioUpdate.Id);
            parametros.Add("@NOME", usuarioUpdate.Nome.Trim());
            parametros.Add("@EMAIL", usuarioUpdate.Email.Trim());
            parametros.Add("@LOGIN", usuarioUpdate.Login.Trim());
            parametros.Add("@ATIVO", usuarioUpdate.Ativo ? 1 : 0);
            parametros.Add("@PRIMEIROACESSO", usuarioUpdate.PrimeiroAcesso ? 1 : 0);
            parametros.Add("@ULTIMOACESSO", DateTime.Now);
            parametros.Add("@PERFILID", usuarioUpdate.PerfilId);
            parametros.Add("@RESETSOLICITADO", usuarioUpdate.ResetSolicitado);

            if (!string.IsNullOrEmpty(usuarioUpdate.Senha) && usuarioUpdate.Senha != usuarioBase.Senha)
                parametros.Add("@SENHA", usuarioUpdate.Senha);
            else
                parametros.Add("@SENHA", usuarioBase.Senha);

            if (usuarioUpdate.Token != usuarioBase.Token && !string.IsNullOrEmpty(usuarioUpdate.Token))
                parametros.Add("@TOKEN", usuarioUpdate.Token);
            else
                parametros.Add("@TOKEN", usuarioBase.Token);

            if (usuarioUpdate.DataUltimoToken != DateTime.MinValue)
                parametros.Add("@DATAULTIMOTOKEN", DateTime.Now);
            else
                parametros.Add("@DATAULTIMOTOKEN", usuarioBase.DataUltimoToken);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }

        public async Task<ListaPaginada<Usuario>> ListarUsuariosAsync(FiltroUsuario filtro, bool paginarRegistros = true)
        {
            var query = @"SELECT A.* FROM (
	                        SELECT U.*,
	                        P.DESCRICAO AS PERFILDESCRICAO,
	                        CASE WHEN (U.ATIVO) = 1 THEN 'ATIVO' ELSE 'INATIVO' END AS STATUSFORMATADO 
	                        FROM USUARIO U
	                        INNER JOIN PERFIL P ON P.ID = U.PERFILID
                        ) AS A ";
            var parametros = new DynamicParameters();

            var where = " WHERE ";
            var and = " AND ";
            var whereInsert = false;

            if (!string.IsNullOrEmpty(filtro.Todos))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @" (
                                UPPER(NOME) LIKE '%' + @TODOS + '%'
                                OR UPPER(EMAIL) LIKE '%' + @TODOS + '%'
                                OR UPPER(LOGIN) LIKE '%' + @TODOS + '%'
                                OR UPPER(PERFILDESCRICAO) LIKE '%' + @TODOS + '%'
                                OR STATUSFORMATADO LIKE '%' + @TODOS + '%'
                            ) ";
                parametros.Add("@TODOS", filtro.Todos.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(NOME) LIKE '%' + @NOME + '%'";
                parametros.Add("@NOME", filtro.Nome.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.Login))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(LOGIN) LIKE '%' + @LOGIN + '%'";
                parametros.Add("@LOGIN", filtro.Login.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.Email))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(EMAIL) LIKE '%' + @EMAIL + '%'";
                parametros.Add("@EMAIL", filtro.Email.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.PerfilDescricao))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(PERFILDESCRICAO) LIKE '%' + @PERFILDESCRICAO + '%'";
                parametros.Add("@PERFILDESCRICAO", filtro.PerfilDescricao.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.StatusFormatado))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(STATUSFORMATADO) LIKE '%' + @STATUSFORMATADO + '%'";
                parametros.Add("@STATUSFORMATADO", filtro.StatusFormatado.Trim().ToUpper());
            }

            if (filtro.Ativos.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"ATIVO = @ATIVO";
                parametros.Add("@ATIVO", filtro.Ativos.Value ? 1 : 0);
            }

            if (filtro.PerfisIds != null)
            {
                if (filtro.PerfisIds.Count() > 0)
                {
                    if (whereInsert == false) { query += where; whereInsert = true; }
                    else query += and;

                    query += @"(";

                    var controleValidacaoOr = 0;

                    foreach (var perfilId in filtro.PerfisIds)
                    {

                        controleValidacaoOr += 1;

                        query += @$"PERFILID = @PERFILID{controleValidacaoOr}";
                        parametros.Add($"@PERFILID{controleValidacaoOr}", perfilId);

                        if (controleValidacaoOr != filtro.PerfisIds.Count())
                            query += " OR ";
                    }

                    query += @") ";
                }
            }

            query += @" ORDER BY";

            switch (filtro.OrdenarPor)
            {
                case CamposUsuarioEnum.Nome:
                    query += @" NOME ";
                    break;
                case CamposUsuarioEnum.Login:
                    query += @" LOGIN ";
                    break;
                case CamposUsuarioEnum.Perfil:
                    query += @" PERFILDESCRICAO ";
                    break;
                case CamposUsuarioEnum.Email:
                    query += @" EMAIL ";
                    break;
                case CamposUsuarioEnum.Status:
                    query += @" STATUSFORMATADO ";
                    break;
                default:
                    query += @" NOME ";
                    break;
            }

            query += filtro.Ordem == AscDescEnum.Asc ? @"ASC " : @"DESC ";

            var queryCount = query;

            if (paginarRegistros)
            {
                query += @"OFFSET (@PAGINA - 1) * @ITENSPAGINA ROWS 
                        FETCH NEXT @ITENSPAGINA ROWS ONLY";

                parametros.Add("@PAGINA", filtro.Pagina);
                parametros.Add("@ITENSPAGINA", filtro.ItensPagina);
            }

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Usuario>(query, parametros);

                queryCount = queryCount.Replace("SELECT A.*", "SELECT COUNT(*)");
                queryCount = queryCount.Replace("ORDER BY NOME ASC", "").Replace("ORDER BY NOME DESC", "");
                queryCount = queryCount.Replace("ORDER BY LOGIN ASC", "").Replace("ORDER BY LOGIN DESC", "");
                queryCount = queryCount.Replace("ORDER BY PERFILDESCRICAO ASC", "").Replace("ORDER BY PERFILDESCRICAO DESC", "");
                queryCount = queryCount.Replace("ORDER BY EMAIL ASC", "").Replace("ORDER BY EMAIL DESC", "");
                queryCount = queryCount.Replace("ORDER BY STATUSFORMATADO ASC", "").Replace("ORDER BY STATUSFORMATADO DESC", "");

                var resultCount = await connection.QueryFirstOrDefaultAsync<int>(queryCount, parametros);

                var paginas = resultCount % filtro.ItensPagina > 0 ? (resultCount / filtro.ItensPagina) + 1 : resultCount / filtro.ItensPagina;
                if (paginas == 0)
                    paginas = 1;

                var response = new ListaPaginada<Usuario>()
                {
                    Lista = result.ToList(),
                    Paginas = paginas,
                    TotalItens = resultCount
                };

                return response;
            }
        }

        public async Task<bool> UsuarioExistenteAsync(string id, string nome)
        {

            var query = @"SELECT ID FROM USUARIO WHERE UPPER(NOME) = @NOME";

            if (!string.IsNullOrEmpty(id))
                query += @" AND ID != @ID";

            var parametros = new DynamicParameters();
            parametros.Add("@NOME", nome.ToUpper());
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<string>(query, parametros);

                return result.Count() > 0;
            }
        }

        public async Task<bool> LoginExistenteAsync(string login)
        {
            var query = @"SELECT ID FROM USUARIO WHERE UPPER(LOGIN) = @LOGIN";

            var parametros = new DynamicParameters();
            parametros.Add("@LOGIN", login.ToUpper());

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<string>(query, parametros);

                return result.Count() > 0;
            }
        }

        public async Task<bool> EmailExistenteAsync(string email)
        {
            var query = @"SELECT ID FROM USUARIO WHERE UPPER(EMAIL) = @EMAIL";

            var parametros = new DynamicParameters();
            parametros.Add("@EMAIL", email.Trim().ToUpper());

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<string>(query, parametros);

                return result.Count() > 0;
            }
        }

        public async Task<Usuario> LoginAsync(string login, string senha)
        {
            var query = @"SELECT ID FROM USUARIO WHERE ATIVO = 1 AND LOGIN = @LOGIN AND SENHA = @SENHA";

            var parametros = new DynamicParameters();
            parametros.Add("@LOGIN", login);
            parametros.Add("@SENHA", CriptografiaSenha.Criptografar(senha));

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryFirstOrDefaultAsync<string>(query, parametros);

                if (!string.IsNullOrEmpty(result))
                {
                    var usuario = await GetAsync(result);

                    return usuario;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> AlterarSenhaAsync(string novaSenha, string token)
        {

            var query = @"SELECT * FROM USUARIO WHERE TOKEN = @TOKEN";

            var parametros = new DynamicParameters();
            parametros.Add("@TOKEN", token);

            using (IDbConnection connection = _connection.Invoke())
            {
                var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                if (usuario != null)
                {
                    usuario.Senha = CriptografiaSenha.Criptografar(novaSenha);
                    usuario.PrimeiroAcesso = false;
                    usuario.ResetSolicitado = false;

                    await UpdateAsync(usuario);

                    return true;
                }
                else
                    return false;
            }
        }

        public async Task<Usuario> AlterarSenhaUsuarioWebAsync(string usuarioId, string novaSenha)
        {

            var query = @"SELECT * FROM USUARIO WHERE ID = @USUARIOID";

            var parametros = new DynamicParameters();
            parametros.Add("@USUARIOID", usuarioId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                if (usuario != null)
                {
                    usuario.Senha = CriptografiaSenha.Criptografar(novaSenha);
                    usuario.PrimeiroAcesso = true;
                    usuario.ResetSolicitado = false;

                    await UpdateAsync(usuario);

                    return usuario;
                }
                else
                    return usuario;
            }
        }

        public async Task<Usuario> AtualizarTokenAsync(Usuario usuario)
        {

            var usuarioUpdate = await GetAsync(usuario.Id);

            usuarioUpdate.Senha = null;

            if (usuario.Token != usuarioUpdate.Token && !string.IsNullOrEmpty(usuario.Token))
                usuarioUpdate.Token = usuario.Token;

            if (usuario.DataUltimoToken != DateTime.MinValue)
                usuarioUpdate.DataUltimoToken = usuario.DataUltimoToken;

            await UpdateAsync(usuarioUpdate);

            return usuarioUpdate;
        }

        public async Task<bool> UsuarioAutorizadoAsync(string token)
        {

            var query = @"SELECT * FROM USUARIO WHERE TOKEN = @TOKEN";

            var parametros = new DynamicParameters();
            parametros.Add("@TOKEN", token);

            using (IDbConnection connection = _connection.Invoke())
            {
                var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                if (usuario == null)
                    return false;

                TimeSpan diferenca = DateTime.Now.Subtract(usuario.DataUltimoToken);
                if (diferenca.TotalMinutes > 20)
                    return false;

                usuario.Token = GeradorToken.GerarToken();
                usuario.DataUltimoToken = DateTime.Now;
                await UpdateAsync(usuario);
                return true;
            }
        }

        public async Task<Usuario> AlterarSenhaUsuarioAsync(string usuarioId, string novaSenha)
        {
            var usuario = await GetAsync(usuarioId);

            if (usuario != null)
            {
                usuario.Senha = CriptografiaSenha.Criptografar(novaSenha);
                await UpdateAsync(usuario);
            }

            return usuario;
        }

        public async Task<Usuario> ConsultarUsuarioPorLoginOuEmailAsync(string busca)
        {
            var query = @"SELECT * FROM USUARIO WHERE UPPER(LOGIN) = @BUSCA OR UPPER(EMAIL) = @BUSCA";

            var parametros = new DynamicParameters();
            parametros.Add("@BUSCA", busca.Trim().ToUpper());


            using (IDbConnection connection = _connection.Invoke())
            {
                var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                if (usuario != null)
                {
                    usuario.ResetSolicitado = true;
                    await UpdateAsync(usuario);
                }

                return usuario;
            }
        }

        public async Task<List<Usuario>> ListarUsuariosAtivosAsync()
        {
            var query = @"SELECT CODIGO, NOME FROM USUARIO WHERE ATIVO = 1 ORDER BY NOME";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Usuario>(query);

                return result.ToList();
            }
        }

        public async Task<Usuario> GetByCodigoAsync(int codigo)
        {
            var query = @"SELECT * FROM USUARIO WHERE CODIGO=@CODIGO";

            var parametros = new DynamicParameters();
            parametros.Add("@CODIGO", codigo);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                return result;
            }
        }
    }
}
