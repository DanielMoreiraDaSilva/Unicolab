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
    public class MateriaCursoRepository : IMateriaCursoRepository
    {
        private readonly Func<IDbConnection> _connection;
        public MateriaCursoRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<string>> GetAllByCursoIdAsync(string cursoId)
        {
            string query = @"SELECT MATERIAID FROM MATERIACURSO WHERE CURSOID = @CURSOID";

            var parametros = new DynamicParameters();
            parametros.Add("@CURSOID", cursoId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<string>(query, parametros);

                return result.ToList();
            }
        }
    }
}
