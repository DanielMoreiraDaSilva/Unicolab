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
    public class ModuloRepository : IModuloRepository
    {
        private readonly Func<IDbConnection> _connection;

        public ModuloRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }
        public async Task<List<Modulo>> GetAllAsync()
        {
            string query = @"SELECT * FROM MODULO";
             
            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Modulo>(query);

                return result.ToList();
            }
        }

        public async Task<List<Modulo>> GetAllByPerfilIdAsync(string perfilId)
        {
            string query = "SELECT * FROM MODULO M INNER JOIN PERFILMODULO P ON M.ID = P.MODULOID AND P.PERFILID = @PERFILID ORDER BY ORDEM";

            var parametros = new DynamicParameters();
            parametros.Add("@PERFILID", perfilId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var result = await connection.QueryAsync<Modulo>(query, parametros);

                return result.ToList();
            }
        }
    }
}
