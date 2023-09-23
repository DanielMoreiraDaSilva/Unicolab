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
    public class UsuarioMateriaRepository : IUsuarioMateriaRepository
    {
        private readonly Func<IDbConnection> _connection;
        public UsuarioMateriaRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<UsuarioMateria>> GetAllAsync()
        {
            string query = @"SELECT * FROM USUARIOMATERIA";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<UsuarioMateria>(query);

                return result.ToList();
            }
        }
        public async Task<List<UsuarioMateria>> GetAllByUsuarioIdAsync(string usuarioId)
        {
            string query = @"SELECT * FROM USUARIOMATERIA WHERE USUARIOID = @USUARIOID";

            var parametros = new DynamicParameters();
            parametros.Add("@USUARIOID", usuarioId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<UsuarioMateria>(query, parametros);

                return result.ToList();
            }
        }

        public async Task AddAsync(UsuarioMateria usuarioMateria)
        {
            var query = @"INSERT INTO USUARIOMATERIA
                            VALUES
                            (
                                @ID,
                                @USUARIOID,
                                @MATERIAID,
                                1,
                                2022,
                                NULL,
                                NULL
                            )";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", Guid.NewGuid().ToString().ToUpper());
            parametros.Add("@USUARIOID", usuarioMateria.UsuarioId);
            parametros.Add("@MATERIAID", usuarioMateria.MateriaId);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }

        public async Task DeleteAsync(string usuarioId)
        {
            var query = @"DELETE FROM USUARIOMATERIA WHERE USUARIOID = @USUARIOID";

            var parametros = new DynamicParameters();
            parametros.Add("@USUARIOID", usuarioId);

            using (IDbConnection connection = _connection.Invoke())
            {
                await connection.ExecuteAsync(query, parametros);
            }
        }
    }
}