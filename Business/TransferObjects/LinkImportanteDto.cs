namespace Business.TransferObjects
{
    public class LinkImportanteDto
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public string Icone { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
        public string StatusFormatado { get; set; }
    }
}