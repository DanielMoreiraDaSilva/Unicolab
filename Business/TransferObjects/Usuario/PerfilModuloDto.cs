namespace Business.TransferObjects
{
    public class PerfilModuloDto
    {
        public string Id { get; set; }
        public string PerfilId { get; set; }
        public string ModuloId { get; set; }
        public string Acesso { get; set; }
        public ModuloDto Modulo { get; set; }
    }
}
