using Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.Filtros
{
    public class FiltroUsuarioContato
    {
        public string Todos { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string PerfilDescricao { get; set; }
        public string StatusFormatado { get; set; }
        public List<string> PerfisIds { get; set; }
        public bool? Ativos { get; set; }
        public CamposUsuarioContatoEnum OrdenarPor { get; set; }
        public AscDescEnum Ordem { get; set; }
        public int Pagina { get; set; }
        public int ItensPagina { get; set; }
    }
}
