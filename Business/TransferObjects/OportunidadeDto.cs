using System;

namespace Business.TransferObjects
{
    public class OportunidadeDto
    {
        public string Id { get; set; }
        public string CursoId { get; set; }
        public string UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Empresa { get; set; }
        public string Descricao { get; set; }
        public decimal? Salario { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Ativo { get; set; }
    }
}
