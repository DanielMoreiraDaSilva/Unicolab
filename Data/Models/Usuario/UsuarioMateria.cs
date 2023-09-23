namespace Data.Models
{
    public class UsuarioMateria
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string MateriaId { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        public bool Concluido { get; set; }
        public int Media { get; set; }
    }
}
