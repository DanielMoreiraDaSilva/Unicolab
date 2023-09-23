namespace Data.Models
{
    public class UsuarioCurso
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string CursoId { get; set; }
        public bool? Ativo { get; set; }
    }
}
