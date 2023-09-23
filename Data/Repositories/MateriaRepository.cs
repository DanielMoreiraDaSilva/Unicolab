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
    public class MateriaRepository : IMateriaRepository
    {
        private readonly Func<IDbConnection> _connection;
        public MateriaRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<Materia>> GetAllAsync()
        {
            string query = @"SELECT * FROM MATERIA";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Materia>(query);

                return result.ToList();
            }
        }

        public async Task<List<Materia>> GetAllByUsuarioAsync(string usuarioId)
        {
            string query = @"SELECT M.* FROM MATERIA M INNER JOIN USUARIOMATERIA UM ON M.ID = UM.MATERIAID AND UM.USUARIOID = @USUARIOID";

            var parametros = new DynamicParameters();
            parametros.Add("@USUARIOID", usuarioId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Materia>(query, parametros);

                return result.ToList();
            }
        }
    }
}
