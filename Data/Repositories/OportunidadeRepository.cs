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
    public class OportunidadeRepository : IOportunidadeRepository
    {
        private readonly Func<IDbConnection> _connection;
        public OportunidadeRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }

        public async Task<List<Oportunidade>> GetAllAsync()
        {
            var query = @"SELECT * FROM OPORTUNIDADE";

            using (IDbConnection connection = _connection.Invoke())
            {
                var Oportunidade = await connection.QueryAsync<Oportunidade>(query);

                return Oportunidade.ToList();
            }
        }
        public async Task<List<Oportunidade>> GetAllByCurso(string cursoId)
        {
            var query = "SELECT * FROM OPORTUNIDADE WHERE CURSOID = @CURSOID";

            var parametros = new DynamicParameters();
            parametros.Add("@CURSOID", cursoId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Oportunidade>(query, parametros);

                return result.ToList();
            }
        }

        public async Task<ListaPaginada<Oportunidade>> ListarOportunidadesAsync(FiltroOportunidade filtro)
        {
            var query = @"SELECT A.* FROM (
	                        SELECT O.*,
	                        CASE WHEN (O.ATIVO) = 1 THEN 'ATIVO' ELSE 'INATIVO' END AS STATUSFORMATADO,
	                        C.DESCRICAO AS CURSODESCRICAO,
	                        U.NOME AS USUARIONOME
	                        FROM OPORTUNIDADE O
                            INNER JOIN CURSO C ON C.ID = O.CURSOID
                            INNER JOIN USUARIO U ON U.ID = O.USUARIOID
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
                                UPPER(A.TITULO) LIKE '%' + @TODOS + '%'
                                OR A.STATUSFORMATADO LIKE '%' + @TODOS + '%'
                            ) ";
                parametros.Add("@TODOS", filtro.Todos.Trim().ToUpper());
            }

            if (filtro.CriadasPorMim.HasValue && filtro.CriadasPorMim.Value)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @" A.USUARIOID = @USUARIOID";
                parametros.Add("@USUARIOID", filtro.UsuarioId);
            }

            if (!string.IsNullOrEmpty(filtro.Titulo))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(A.TITULO) LIKE '%' + @TITULO + '%'";
                parametros.Add("@TITULO", filtro.Titulo.Trim().ToUpper());
            }

            if (!string.IsNullOrEmpty(filtro.StatusFormatado))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(A.STATUSFORMATADO) LIKE '%' + @STATUSFORMATADO + '%'";
                parametros.Add("@STATUSFORMATADO", filtro.StatusFormatado.Trim().ToUpper());
            }

            if (filtro.Ativos.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"A.ATIVO = @ATIVO";
                parametros.Add("@ATIVO", filtro.Ativos.Value ? 1 : 0);
            }

            if (filtro.ListaCurso.Count > 0)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @" A.CURSOID IN @LISTACURSO ";
                parametros.Add("@LISTACURSO", filtro.ListaCurso);
            }

            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"A.DATAINICIO >= @DATAINICIO AND A.DATAFIM <= @DATAFIM";
                parametros.Add("@DATAINICIO", filtro.DataInicio.Value);
                parametros.Add("@DATAFIM", filtro.DataFim.Value);
            }
            else if (filtro.DataInicio.HasValue && !filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"A.DATAINICIO >= @DATAINICIO";
                parametros.Add("@DATAINICIO", filtro.DataInicio.Value);
            }
            else if (!filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"A.DATAFIM <= @DATAFIM";
                parametros.Add("@DATAFIM", filtro.DataFim.Value);
            }
            else
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"(A.DATAFIM > GETDATE() OR A.DATAFIM IS NULL)";
            }

            var queryCount = query;

            query += @" ORDER BY A.DATAINICIO ASC OFFSET (@PAGINA - 1) * @ITENSPAGINA ROWS 
                    FETCH NEXT @ITENSPAGINA ROWS ONLY";

            parametros.Add("@PAGINA", filtro.Pagina);
            parametros.Add("@ITENSPAGINA", filtro.ItensPagina);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Oportunidade>(query, parametros);

                queryCount = queryCount.Replace("SELECT A.*", "SELECT COUNT(*)");

                queryCount = queryCount.Replace("ORDER BY A.DATAINICIO ASC", "");

                var resultCount = await connection.QueryFirstOrDefaultAsync<int>(queryCount, parametros);

                var paginas = resultCount % filtro.ItensPagina > 0 ? (resultCount / filtro.ItensPagina) + 1 : resultCount / filtro.ItensPagina;
                if (paginas == 0)
                    paginas = 1;

                var response = new ListaPaginada<Oportunidade>()
                {
                    Lista = result.ToList(),
                    Paginas = paginas,
                    TotalItens = resultCount
                };

                return response;
            }
        }

        public async Task UpdateAsync(Oportunidade oportunidade)
        {
            var query = @"UPDATE OPORTUNIDADE
                            SET 
                            CURSOID=@CURSOID,
                            TITULO=@TITULO,
                            EMPRESA=@EMPRESA,    
                            DESCRICAO=@DESCRICAO, 
                            SALARIO=@SALARIO,
                            DATAFIM=@DATAFIM,
                            ATIVO=@ATIVO
                            WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", oportunidade.Id);
            parametros.Add("@CURSOID", oportunidade.CursoId);
            parametros.Add("@TITULO", oportunidade.Titulo);
            parametros.Add("@EMPRESA", oportunidade.Empresa);
            parametros.Add("@DESCRICAO", oportunidade.Descricao);
            parametros.Add("@SALARIO", oportunidade.Salario);
            parametros.Add("@DATAFIM", oportunidade.DataFim);
            parametros.Add("@ATIVO", oportunidade.Ativo ? 1 : 0);

            using (IDbConnection connetion = _connection.Invoke())
            {
                await connetion.ExecuteAsync(query, parametros);
            }
        }
        public async Task AddAsync(Oportunidade oportunidade)
        {
            var query = @"INSERT INTO OPORTUNIDADE
                            VALUES
                            (
                            @ID,
                            @CURSOID,
                            @USUARIOID,
                            @TITULO,
                            @EMPRESA,    
                            @DESCRICAO,
                            @SALARIO,
                            @DATAINICIO,
                            @DATAFIM,
                            1
                            )";

            var parametros = new DynamicParameters();
            var id = Guid.NewGuid().ToString().ToUpper();
            parametros.Add("@ID", id);
            parametros.Add("@CURSOID", oportunidade.CursoId); 
            parametros.Add("@USUARIOID", oportunidade.UsuarioId);
            parametros.Add("@TITULO", oportunidade.Titulo);
            parametros.Add("@EMPRESA", oportunidade.Empresa);
            parametros.Add("@DESCRICAO", oportunidade.Descricao);
            parametros.Add("@SALARIO", oportunidade.Salario);
            parametros.Add("@DATAINICIO", DateTime.Now);
            parametros.Add("@DATAFIM", oportunidade.DataFim);
            using (IDbConnection connetion = _connection.Invoke())
            {
                await connetion.ExecuteAsync(query, parametros);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var query = @"DELETE FROM OPORTUNIDADE WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }
    }
}
