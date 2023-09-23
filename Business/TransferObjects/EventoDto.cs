using System;

namespace Business.TransferObjects
{
	public class EventoDto
	{
        public string Id { get; set; }
        public string CursoId { get; set; }
        public string UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string TipoEventoId { get; set; }
        public string Descricao { get; set; }
        public int Pontos { get; set; }
        public string Palestrante { get; set; }
        public string LocalEvento { get; set; }
        public DateTime DataHorarioInicio { get; set; }
        public int Duracao { get; set; }
        public bool Ativo { get; set; }
        public string DescricaoArea { get; set; }
	}
}