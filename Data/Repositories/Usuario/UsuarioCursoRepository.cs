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
    public class UsuarioCursoRepository : IUsuarioCursoRepository
    {
        private readonly Func<IDbConnection> _connection;
        public UsuarioCursoRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<UsuarioCurso>> GetAllAsync()
        {
            string query = @"SELECT * FROM USUARIOCURSO";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<UsuarioCurso>(query);

                return result.ToList();
            }
        }

        public async Task<List<UsuarioCurso>> GetAllByUsuarioIdAsync(string usuarioId)
        {
            string query = @"SELECT * FROM USUARIOCURSO WHERE USUARIOID = @USUARIOID";

            var parametros = new DynamicParameters();
            parametros.Add("@USUARIOID", usuarioId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<UsuarioCurso>(query, parametros);

                return result.ToList();
            }
        }
        
        public async Task AddAsync(UsuarioCurso usuarioCurso)
        {
            var query = @"INSERT INTO USUARIOCURSO
                            VALUES
                            (
                            @ID,
                            @USUARIOID,
                            @CURSOID,
                            1
                            )";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", Guid.NewGuid().ToString().ToUpper());
            parametros.Add("@USUARIOID", usuarioCurso.UsuarioId);
            parametros.Add("@CURSOID", usuarioCurso.CursoId);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }

        public async Task DeleteByUsuarioId(string usuarioId)
        {
            var query = @"DELETE FROM USUARIOCURSO WHERE USUARIOID = @USUARIOID";

            var parametros = new DynamicParameters();
            parametros.Add("@USUARIOID", usuarioId);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }
    }
}
