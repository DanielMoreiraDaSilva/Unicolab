using Dapper;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RespostaRepository : IRespostaRepository
    {
        private readonly Func<IDbConnection> _connection;
        public RespostaRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }

        public async Task<List<Resposta>> GetAllByDuvidaId(string duvidaId)
        {
            var query = @"SELECT R.*, U.NOME NOMEUSUARIO
                          FROM RESPOSTA R
                          INNER JOIN USUARIO U 
                          ON U.ID = R.USUARIOID
                          WHERE R.DUVIDAID = @DUVIDAID ORDER BY R.MELHORRESPOSTA DESC, R.DATAHORA DESC";

            var parametros = new DynamicParameters();
            parametros.Add("@DUVIDAID", duvidaId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Resposta>(query, parametros);

                return result.ToList();
            }
        }

        public async Task AddAsync(Resposta resposta)
        {
            var query = @"INSERT INTO RESPOSTA
                            VALUES
                            (
                                @ID,
                                @DESCRICAO,
                                @MELHORRESPOSTA,
                                @DATAHORA,
                                @USUARIOID,
                                @DUVIDAID
                            )
                            ";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", Guid.NewGuid().ToString().ToUpper());
            parametros.Add("@DESCRICAO", resposta.Descricao);
            parametros.Add("@MELHORRESPOSTA", 0);
            parametros.Add("@DATAHORA", DateTime.Now);
            parametros.Add("@USUARIOID", resposta.UsuarioId);
            parametros.Add("@DUVIDAID", resposta.DuvidaId);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }

        public async Task UpdateAsync(Resposta resposta)
        {
            var query = @"UPDATE RESPOSTA
                            SET
                                DESCRICAO=@DESCRICAO,
                                MELHORRESPOSTA=@MELHORRESPOSTA
                            WHERE ID=@ID
                            ";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", resposta.Id);
            parametros.Add("@DESCRICAO", resposta.Descricao);
            parametros.Add("@MELHORRESPOSTA", resposta.MelhorResposta ? 1 : 0);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var query = @"DELETE FROM RESPOSTA WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }
    }
}
