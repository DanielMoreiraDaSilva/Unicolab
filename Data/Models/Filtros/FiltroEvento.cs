using Data.Models.Enums;
using System;
using System.Collections.Generic;

namespace Data.Models.Filtros
{
    public class FiltroEvento
    {
        public string Todos { get; set; }
        public string CursoId { get; set; }
        public string Titulo { get; set; }
        public List<string> ListaTipoEventoId { get; set; }
        public string LocalEvento { get; set; }
        public string Palestrante { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public CamposEventoEnum OrdenarPor { get; set; }
        public AscDescEnum Ordem { get; set; }
        public int Pagina { get; set; }
        public int ItensPagina { get; set; }
    }
}
