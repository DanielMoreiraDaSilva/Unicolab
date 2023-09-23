using System;
using System.Collections.Generic;

namespace Data.Models.Filtros
{
    public class FiltroDuvida
    {
        public List<string> ListaMateria { get; set; }
        public string UsuarioId { get; set; }
        public string PerfilId { get; set; }
        public string Pergunta { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Pagina { get; set; }
        public int ItensPagina { get; set; }
        public bool? MinhasDuvidas { get; set; }
    }
}
