using Dapper;
using Data.Constantes;
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
    public class EventoRepository : IEventoRepository
    {
        private readonly Func<IDbConnection> _connection;
        public EventoRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<Evento>> GetAllAsync()
        {
            string query = @"SELECT * FROM EVENTO";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Evento>(query);

                return result.ToList();
            }
        }
        public async Task AddAsync(Evento evento)
        {
            var query = @"INSERT INTO EVENTO
                            VALUES
                            (
                            @ID,
                            @CURSOID,
                            @USUARIOID,
                            @TITULO,
                            @TIPOEVENTOID,    
                            @DESCRICAO,
                            @PONTOS,
                            @PALESTRANTE,
                            @LOCALEVENTO,
                            @DATAHORARIOINICIO,
                            @DURACAO,
                            1
                            )";

            var parametros = new DynamicParameters();
            var id = Guid.NewGuid().ToString().ToUpper();
            parametros.Add("@ID", id);
            parametros.Add("@CURSOID", evento.CursoId);
            parametros.Add("@USUARIOID", evento.UsuarioId);
            parametros.Add("@TITULO", evento.Titulo);
            parametros.Add("@TIPOEVENTOID", evento.TipoEventoId);
            parametros.Add("@DESCRICAO", evento.Descricao);
            parametros.Add("@PONTOS", evento.Pontos);
            parametros.Add("@PALESTRANTE", evento.Palestrante);
            parametros.Add("@LOCALEVENTO", evento.LocalEvento);
            parametros.Add("@DATAHORARIOINICIO", evento.DataHorarioInicio);
            parametros.Add("@DURACAO", evento.Duracao);

            using (IDbConnection connetion = _connection.Invoke())
            {
                await connetion.ExecuteAsync(query, parametros);
            }
        }
        public async Task UpdateAsync(Evento evento)
        {
            var query = @"UPDATE EVENTO
                        SET 
                        CURSOID=@CURSOID,
                        TITULO=@TITULO,
                        TIPOEVENTOID=@TIPOEVENTOID,    
                        DESCRICAO=@DESCRICAO, 
                        PONTOS=@PONTOS,
                        PALESTRANTE=@PALESTRANTE,
                        LOCALEVENTO=@LOCALEVENTO,
                        DATAHORARIOINICIO=@DATAHORARIOINICIO,
                        DURACAO=@DURACAO,
                        ATIVO=@ATIVO
                        WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", evento.Id);
            parametros.Add("@CURSOID", evento.CursoId);
            parametros.Add("@TITULO", evento.Titulo);
            parametros.Add("@TIPOEVENTOID", evento.TipoEventoId);
            parametros.Add("@DESCRICAO", evento.Descricao);
            parametros.Add("@PONTOS", evento.Pontos);
            parametros.Add("@PALESTRANTE", evento.Palestrante);
            parametros.Add("@LOCALEVENTO", evento.LocalEvento);
            parametros.Add("@DATAHORARIOINICIO", evento.DataHorarioInicio);
            parametros.Add("@DURACAO", evento.Duracao);
            parametros.Add("@Ativo", evento.Ativo ? 1 : 0);

            using (IDbConnection connetion = _connection.Invoke())
            {
                await connetion.ExecuteAsync(query, parametros);
            }
        }

        public async Task<List<Evento>> GetAllByCurso(string cursoId)
        {
            var query = "SELECT * FROM EVENTO WHERE CURSOID = @CURSOID";

            var parametros = new DynamicParameters();
            parametros.Add("@CURSOID", cursoId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Evento>(query, parametros);

                return result.ToList();
            }
        }

        public async Task<ListaPaginada<Evento>> ListarEventosAsync(FiltroEvento filtro)
        {
            var query = @"SELECT A.* FROM (
	                        SELECT O.*,
                            A.DESCRICAO DESCRICAOAREA,
                            A.ID AREAID,
	                        CASE WHEN (O.ATIVO) = 1 THEN 'ATIVO' ELSE 'INATIVO' END AS STATUSFORMATADO
	                        FROM EVENTO O
	                        INNER JOIN CURSO C ON O.CURSOID = C.ID
	                        INNER JOIN AREA A ON C.AREAID = A.ID
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

            if (filtro.ListaTipoEventoId.Count > 0)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @" TIPOEVENTOID IN @LISTATIPOEVENTOID ";
                parametros.Add("@LISTATIPOEVENTOID", filtro.ListaTipoEventoId);
            }

            if (!string.IsNullOrEmpty(filtro.LocalEvento))
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"UPPER(LOCALEVENTO) LIKE '%' + @LOCALEVENTO + '%'";
                parametros.Add("@LOCALEVENTO", filtro.LocalEvento.Trim().ToUpper());
            }

            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"DATAHORARIOINICIO BETWEEN @DATAINICIO AND @DATAFIM";
                parametros.Add("@DATAINICIO", filtro.DataInicio.Value);
                parametros.Add("@DATAFIM", filtro.DataFim.Value);
            }
            else if (filtro.DataInicio.HasValue && !filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"DATAHORARIOINICIO >= @DATAINICIO";
                parametros.Add("@DATAINICIO", filtro.DataInicio.Value);
            }
            else if (!filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                if (whereInsert == false) { query += where; whereInsert = true; }
                else query += and;
                query += @"DATAHORARIOINICIO <= @DATAFIM";
                parametros.Add("@DATAFIM", filtro.DataFim.Value);
            }

            //query += @" ORDER BY";

            //switch (filtro.OrdenarPor)
            //{
            //    case CamposEventoEnum.Titulo:
            //        query += @" TITULO ";
            //        break;
            //    case CamposEventoEnum.DataInicio:
            //        query += @" DATAHORARIOINICIO ";
            //        break;
            //    case CamposEventoEnum.Duracao:
            //        query += @" DURACAO ";
            //        break;
            //    case CamposEventoEnum.Status:
            //        query += @" STATUSFORMATADO ";
            //        break;
            //    default:
            //        query += @" DATAHORARIOINICIO ";
            //        break;
            //}

            //query += filtro.Ordem == AscDescEnum.Asc ? @"ASC " : @"DESC ";

            var queryCount = query;

            query += @" ORDER BY DATAHORARIOINICIO ASC OFFSET (@PAGINA - 1) * @ITENSPAGINA ROWS 
                    FETCH NEXT @ITENSPAGINA ROWS ONLY";

            parametros.Add("@PAGINA", filtro.Pagina);
            parametros.Add("@ITENSPAGINA", filtro.ItensPagina);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Evento>(query, parametros);

                queryCount = queryCount.Replace("SELECT A.*", "SELECT COUNT(*)");

                queryCount = queryCount.Replace("ORDER BY TITULO ASC", "")
                                       .Replace("ORDER BY TITULO DESC", "")
                                       .Replace("ORDER BY DATAHORARIOINICIO ASC", "")
                                       .Replace("ORDER BY DATAHORARIOINICIO DESC", "")
                                       .Replace("ORDER BY DURACAO ASC", "")
                                       .Replace("ORDER BY DURACAO DESC", "")
                                       .Replace("ORDER BY STATUSFORMATADO ASC", "")
                                       .Replace("ORDER BY STATUSFORMATADO DESC", "");

                var resultCount = await connection.QueryFirstOrDefaultAsync<int>(queryCount, parametros);

                var paginas = resultCount % filtro.ItensPagina > 0 ? (resultCount / filtro.ItensPagina) + 1 : resultCount / filtro.ItensPagina;
                if (paginas == 0)
                    paginas = 1;

                var response = new ListaPaginada<Evento>()
                {
                    Lista = result.ToList(),
                    Paginas = paginas,
                    TotalItens = resultCount
                };

                return response;

            }
        }

        public async Task DeleteAsync(string id)
        {
            var query = @"DELETE FROM EVENTO WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }
    }
}