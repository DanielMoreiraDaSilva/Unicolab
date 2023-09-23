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
    public class PerfilRepository : IPerfilRepository
    {
        private readonly Func<IDbConnection> _connection;
       
        public PerfilRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }   
        public async Task<List<Perfil>> GetAllAsync()
        {
            string query = @"SELECT * FROM PERFIL";

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Perfil>(query);

                return result.ToList();
            }
        }

        public async Task<Perfil> GetAsync(string id)
        {
            string query = @"SELECT * FROM PERFIL WHERE ID=@ID";

            var parametros = new DynamicParameters();
            parametros.Add("@ID", id);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryFirstOrDefaultAsync<Perfil>(query, parametros);

                return result;
            }
        }

    }

}
