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
    public class EventoCursoRepository : IEventoCursoRepository
    {
        private readonly Func<IDbConnection> _connection;
        public EventoCursoRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }

        public async Task<List<EventoCurso>> GetAllAsync()
        {
            var query = @"SELECT * FROM EVENTOCURSO";

            using (IDbConnection connection = _connection.Invoke())
            {
                var eventoCurso = await connection.QueryAsync<EventoCurso>(query);

                return eventoCurso.ToList();
            }
        }
    }
}
