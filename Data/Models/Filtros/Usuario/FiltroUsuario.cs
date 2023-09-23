using Data.Models.Enums;
using System.Collections.Generic;

namespace Data.Models.Filtros
{
    public class FiltroUsuario
    {
        public string Todos { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string PerfilDescricao { get; set; }
        public string StatusFormatado { get; set; }
        public List<string> PerfisIds { get; set; }
        public bool? Ativos { get; set; }
        public CamposUsuarioEnum OrdenarPor { get; set; }
        public AscDescEnum Ordem { get; set; }
        public int Pagina { get; set; }
        public int ItensPagina { get; set; }
    }
}
