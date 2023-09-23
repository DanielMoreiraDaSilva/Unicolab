using Dapper;
using Data.Interfaces;
using Data.Models;
using Data.Models.Enums;
using Data.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class LinkImportanteRepository : ILinkImportanteRepository
    {
        private readonly Func<IDbConnection> _connection;
        public LinkImportanteRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<LinkImportante>> GetAllAsync()
        {
            string query = @"SELECT * FROM LINKIMPORTANTE WHERE ATIVO = 1 ORDER BY ORDEM";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<LinkImportante>(query);

                return result.ToList();
            }
        }
        public async Task<ListaPaginada<LinkImportante>> ListarLinksAsync(FiltroLinkImportante filtro)
        {
            var query = @"SELECT A.* FROM (
	                        SELECT L.*,
	                        CASE WHEN (L.ATIVO) = 1 THEN 'ATIVO' ELSE 'INATIVO' END AS STATUSFORMATADO 
	                        FROM LINKIMPORTANTE L
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
                                UPPER(TITULO) LIKE '%' + @TODOS + '%'
                                OR UPPER(URL) LIKE '%' + @TODOS + '%'
                                OR STATUSFORMATADO LIKE '%' + @TODOS + '%'
                            ) ";
                parametros.Add("@TODOS", filtro.Todos.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.Titulo))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(TITULO) LIKE '%' + @TITULO + '%'";
                parametros.Add("@TITULO", filtro.Titulo.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.Url))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(URL) LIKE '%' + @URL + '%'";
                parametros.Add("@URL", filtro.Url.Trim().ToUpper());
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

            query += @" ORDER BY";

            switch (filtro.OrdenarPor)
            {
                case CamposLinkImportanteEnum.Titulo:
                    query += @" TITULO ";
                    break;
                case CamposLinkImportanteEnum.Url:
                    query += @" URL ";
                    break;
                case CamposLinkImportanteEnum.Status:
                    query += @" STATUSFORMATADO ";
                    break;
                default:
                    query += @" TITULO ";
                    break;
            }

            query += filtro.Ordem == AscDescEnum.Asc ? @"ASC " : @"DESC ";

            var queryCount = query;

            query += @"OFFSET (@PAGINA - 1) * @ITENSPAGINA ROWS 
                    FETCH NEXT @ITENSPAGINA ROWS ONLY";

            parametros.Add("@PAGINA", filtro.Pagina);
            parametros.Add("@ITENSPAGINA", filtro.ItensPagina);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<LinkImportante>(query, parametros);

                queryCount = queryCount.Replace("SELECT A.*", "SELECT COUNT(*)");

                queryCount = queryCount.Replace("ORDER BY TITULO ASC", "")
                                       .Replace("ORDER BY TITULO DESC", "")
                                       .Replace("ORDER BY URL ASC", "")
                                       .Replace("ORDER BY URL DESC", "")
                                       .Replace("ORDER BY STATUSFORMATADO ASC", "")
                                       .Replace("ORDER BY STATUSFORMATADO DESC", "");

                var resultCount = await connection.QueryFirstOrDefaultAsync<int>(queryCount, parametros);

                var paginas = resultCount % filtro.ItensPagina > 0 ? (resultCount / filtro.ItensPagina) + 1 : resultCount / filtro.ItensPagina;
                if (paginas == 0)
                    paginas = 1;

                var response = new ListaPaginada<LinkImportante>()
                {
                    Lista = result.ToList(),
                    Paginas = paginas,
                    TotalItens = resultCount
                };

                return response;
            }
        }

        public async Task AddAsync(LinkImportante linkImportante)
        {
            var query = @"INSERT INTO LINKIMPORTANTE
                            VALUES
                            (
                            @ID,
                            @TITULO,
                            @DESCRICAO,
                            @URL,
                            '',
                            (SELECT MAX(ORDEM) + 1 FROM LINKIMPORTANTE),
                            1
                            )";

            var parametros = new DynamicParameters();
            var id = Guid.NewGuid().ToString().ToUpper();
            parametros.Add("@ID", id);
            parametros.Add("@TITULO", linkImportante.Titulo);
            parametros.Add("@DESCRICAO", linkImportante.Descricao);
            parametros.Add("@URL", linkImportante.Url);

            using (IDbConnection connetion = _connection.Invoke())
            {
                await connetion.ExecuteAsync(query, parametros);
            }
        }

        public async Task UpdateAsync(LinkImportante linkImportante)
        {
            var query = @"UPDATE LINKIMPORTANTE
                            SET 
                            TITULO=@TITULO, 
                            DESCRICAO=@DESCRICAO, 
                            URL=@URL,
                            ATIVO=@ATIVO
                            WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", linkImportante.Id);
            parametros.Add("@TITULO", linkImportante.Titulo);
            parametros.Add("@DESCRICAO", linkImportante.Descricao);
            parametros.Add("@URL", linkImportante.Url);
            parametros.Add("@ATIVO", linkImportante.Ativo ? 1 : 0);

            using (IDbConnection connetion = _connection.Invoke())
            {
                await connetion.ExecuteAsync(query, parametros);
            }
        }

        public async Task<bool> LinkExistenteAsync(string id, string link)
        {
            var query = @"SELECT ID FROM LINKIMPORTANTE WHERE UPPER(URL) = @URL";

            if (!string.IsNullOrEmpty(id))
                query += @" AND ID != @ID";

            var parametros = new DynamicParameters();
            parametros.Add("@URL", link?.Trim().ToUpper());
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<string>(query, parametros);

                return result.Count() > 0;
            }
        }

        public async Task<bool> TituloExistenteAsync(string id, string titulo)
        {
            var query = @"SELECT ID FROM LINKIMPORTANTE WHERE UPPER(TITULO) = @TITULO";

            if (!string.IsNullOrEmpty(id))
                query += @" AND ID != @ID";

            var parametros = new DynamicParameters();
            parametros.Add("@TITULO", titulo?.Trim().ToUpper());
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<string>(query, parametros);

                return result.Count() > 0;
            }
        }
    }
}