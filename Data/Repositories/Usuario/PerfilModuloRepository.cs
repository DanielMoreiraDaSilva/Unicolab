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
    public class PerfilModuloRepository : IPerfilModuloRepository
    {
        private readonly Func<IDbConnection> _connection;
        public PerfilModuloRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<PerfilModulo>> GetAllAsync()
        {
            string query = @"SELECT * FROM PERFILMODULO";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<PerfilModulo>(query);

                return result.ToList();
            }
        }
    }
}
