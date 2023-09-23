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
    public class CursoRepository : ICursoRepository
    {
        private readonly Func<IDbConnection> _connection;
        public CursoRepository(Func<IDbConnection> connection)
        {
            _connection = connection;
        }

        public async Task<List<Curso>> GetAllAsync(string usuarioId)
        {
            var query = @"SELECT * FROM CURSO 
                            WHERE AREAID IN (
                            SELECT AREA.ID FROM AREA 
                            INNER JOIN CURSO 
                            ON AREA.ID = CURSO.AREAID 
                            INNER JOIN USUARIOCURSO C 
                            ON C.CURSOID = CURSO.ID 
                            AND C.USUARIOID = @USUARIOID)";

            var parametros = new DynamicParameters();
            parametros.Add("@USUARIOID", usuarioId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var curso = await connection.QueryAsync<Curso>(query, parametros);

                return curso.ToList();
            }
        }

        public async Task<List<Curso>> GetByListIdAsync(List<string> cursoListaId)
        {
            var query = @"SELECT * FROM CURSO WHERE ID IN @LISTAID";

            var parametros = new DynamicParameters();
            parametros.Add("@LISTAID", cursoListaId);

            using (IDbConnection connection = _connection.Invoke())
            {
                var curso = await connection.QueryAsync<Curso>(query, parametros);

                return curso.ToList();
            }
        }
    }
}
