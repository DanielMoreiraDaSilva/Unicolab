using Dapper;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
        public class OportunidadeCursoRepository : IOportunidadeCursoRepository
        {
            private readonly Func<IDbConnection> _connection;
            public OportunidadeCursoRepository(Func<IDbConnection> connection)
            {
                _connection = connection;
            }
            public async Task<List<OportunidadeCurso>> GetAllAsync()
            {
                string query = @"SELECT 
                                 * 
                                 FROM 
                                 OPORTUNIDADECURSO";

                using (IDbConnection connection = _connection.Invoke())
                {
                    var result = await connection.QueryAsync<OportunidadeCurso>(query);

                    return result.ToList();
                }
            }
        }
}
