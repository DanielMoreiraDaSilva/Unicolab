using Dapper;
using Data.Constantes;
using Data.Interfaces;
using Data.Models;
using Data.Models.Filtros;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class DuvidaRepository : IDuvidaRepository
    {
        private readonly Func<IDbConnection> _connection;
        public DuvidaRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }

        public async Task<ListaPaginada<Duvida>> ListarDuvidasAsync(FiltroDuvida filtro)
        {
            var query = @"SELECT D.*, 
                            U.NOME NOMEUSUARIO, 
                            M.DESCRICAO NOMEMATERIA,
                            (SELECT COUNT(*) FROM RESPOSTA WHERE DUVIDAID = D.ID) QUANTIDADERESPOSTA
                            FROM DUVIDA D 
                            INNER JOIN USUARIO U 
                            ON U.ID = D.USUARIOID
                            INNER JOIN MATERIA M
                            ON M.ID = D.MATERIAID";

            var parametros = new DynamicParameters();

            var where = " WHERE ";
            var and = " AND ";
            var whereInsert = false;

            if (filtro.MinhasDuvidas.HasValue && filtro.MinhasDuvidas.Value)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @" D.USUARIOID = @USUARIOID";
                parametros.Add("@USUARIOID", filtro.UsuarioId);
            }

            if (filtro.ListaMateria.Count > 0 && filtro.PerfilId != Resources.PERFIL_ADMIN)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @" D.MATERIAID IN @LISTAMATERIA ";
                parametros.Add("@LISTAMATERIA", filtro.ListaMateria);
            }

            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"D.DATAHORA BETWEEN @DATAINICIO AND @DATAFIM";
                parametros.Add("@DATAINICIO", filtro.DataInicio.Value);
                parametros.Add("@DATAFIM", filtro.DataFim.Value);
            }
            else if (filtro.DataInicio.HasValue && !filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"D.DATAHORA BETWEEN @DATAINICIO AND D.DATAHORA";
                parametros.Add("@DATAINICIO", filtro.DataInicio.Value);
            }
            else if (!filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"D.DATAHORA BETWEEN D.DATAHORA AND @DATAFIM";
                parametros.Add("@DATAFIM", filtro.DataFim.Value);
            }

            if (!string.IsNullOrEmpty(filtro.Pergunta))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;

                var palavras = filtro.Pergunta.Split(" ");

                if(palavras.Length == 1)
                {
                    query += @$"CONTAINS(D.PERGUNTA, 'FORMSOF (INFLECTIONAL, {palavras[0]})') OR D.PERGUNTA LIKE '%{palavras[0]}%'";
                }
                else
                {
                    var contains = @"CONTAINS(D.PERGUNTA, '";
                    var likes = " OR ";

                    for (int i = 0; i < palavras.Length; i++)
                    {
                        if (i > 0) 
                        {
                            contains += " | ";
                            likes += " OR ";
                        }

                        contains += $"(FORMSOF (INFLECTIONAL, {palavras[i]})";
                        likes += $"(D.PERGUNTA LIKE '%' + '{palavras[i]}' + '%'";

                        var controlador = i + 1;

                        while(controlador < (palavras.Length - 1))
                        {
                            controlador++;
                            contains += $" & FORMSOF (INFLECTIONAL, {palavras[controlador]})";
                            likes += $" AND D.PERGUNTA LIKE '%{palavras[controlador]}%'";
                        }

                        contains += ")";
                        likes += ")";
                    }

                    contains += "')";
                    query += contains;
                    query += likes;
                }
            }

            query += " ORDER BY D.DATAHORA DESC ";

            var queryCount = query;

            query += @"OFFSET (@PAGINA - 1) * @ITENSPAGINA ROWS 
                    FETCH NEXT @ITENSPAGINA ROWS ONLY";

            parametros.Add("@PAGINA", filtro.Pagina);
            parametros.Add("@ITENSPAGINA", filtro.ItensPagina);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Duvida>(query, parametros);

                queryCount = queryCount.Replace("D.*,", "COUNT(*)")
                                       .Replace("U.NOME NOMEUSUARIO,", "")
                                       .Replace("M.DESCRICAO NOMEMATERIA,", "")
                                       .Replace("(SELECT COUNT(*) FROM RESPOSTA WHERE DUVIDAID = D.ID) QUANTIDADERESPOSTA", "");

                queryCount = queryCount.Replace(" ORDER BY D.DATAHORA DESC ", "");

                var resultCount = await connection.QueryFirstOrDefaultAsync<int>(queryCount, parametros);

                var paginas = resultCount % filtro.ItensPagina > 0 ? (resultCount / filtro.ItensPagina) + 1 : resultCount / filtro.ItensPagina;
                if (paginas == 0)
                    paginas = 1;

                var response = new ListaPaginada<Duvida>()
                {
                    Lista = result.ToList(),
                    Paginas = paginas,
                    TotalItens = resultCount
                };

                return response;
            }
        }

        public async Task<string> AddAsync(Duvida duvida)
        {
            var query = @"INSERT INTO DUVIDA
                            VALUES
                            (
                            @ID,
                            @PERGUNTA,
                            @PONTOS,
                            @DATAHORA,
                            @USUARIOID,
                            @MATERIAID
                            )";

            var parametros = new DynamicParameters();
            var id = Guid.NewGuid().ToString().ToUpper();
            parametros.Add("@ID", id);
            parametros.Add("@PERGUNTA", duvida.Pergunta);
            parametros.Add("@PONTOS", duvida.Pontos);
            parametros.Add("@DATAHORA", DateTime.Now);
            parametros.Add("@USUARIOID", duvida.UsuarioId);
            parametros.Add("@MATERIAID", duvida.MateriaId);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);

                return id;
            }
        }

        public async Task UpdateAsync(Duvida duvida)
        {
            var query = @"UPDATE DUVIDA
                            SET
                            PERGUNTA=@PERGUNTA,
                            PONTOS=@PONTOS,
                            MATERIAID=@MATERIAID
                            WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", duvida.Id);
            parametros.Add("@PERGUNTA", duvida.Pergunta);
            parametros.Add("@PONTOS", duvida.Pontos);
            parametros.Add("@MATERIAID", duvida.MateriaId);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }

        public async Task<Duvida> GetAsync(string id)
        {
            var query = @"SELECT * FROM DUVIDA WHERE ID = @ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryFirstOrDefaultAsync<Duvida>(query, parametros);

                return result;
            }
        }

        public async Task DeleteAsync(string id)
        {
            var query = @"DELETE FROM DUVIDA WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }
    }
}
