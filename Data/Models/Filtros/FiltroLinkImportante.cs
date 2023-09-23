using Data.Models.Enums;

namespace Data.Models.Filtros
{
    public class FiltroLinkImportante
    {
        public string Todos { get; set; }
        public string Titulo { get; set; }
        public string Url { get; set; }
        public string StatusFormatado { get; set; }
        public bool? Ativos { get; set; }
        public CamposLinkImportanteEnum OrdenarPor { get; set; }
        public AscDescEnum Ordem { get; set; }
        public int Pagina { get; set; }
        public int ItensPagina { get; set; }
    }
}
