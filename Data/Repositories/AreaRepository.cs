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
    public class AreaRepository : IAreaRepository
    {
        private readonly Func<IDbConnection> _connection;
        public AreaRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<Area>> GetAllAsync()
        {
            string query = @"SELECT * FROM AREA";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Area>(query);

                return result.ToList();
            }
        }
    }
}