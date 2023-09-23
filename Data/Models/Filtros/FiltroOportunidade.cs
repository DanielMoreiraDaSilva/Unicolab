using System;
using System.Collections.Generic;

namespace Data.Models.Filtros
{
    public class FiltroOportunidade
    {
        public string Todos { get; set; }
        public string Titulo { get; set; }
        public string StatusFormatado { get; set; }
        public string UsuarioId { get; set; }
        public List<string> ListaCurso { get; set; }
        public bool? Ativos { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Pagina { get; set; }
        public int ItensPagina { get; set; }
        public bool? CriadasPorMim { get; set; }
    }
}
