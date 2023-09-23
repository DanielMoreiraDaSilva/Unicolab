using System;

namespace Data.Models
{
    public class Resposta
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public bool MelhorResposta { get; set; }
        public DateTime DataHora { get; set; }
        public string UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string DuvidaId { get; set; }
    }
}
