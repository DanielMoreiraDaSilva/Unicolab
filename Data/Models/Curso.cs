using System;

namespace Data.Models
{
    public class Curso
    {
        public string Id { get; set; }
        public string AreaId { get; set; }
        public string Descricao { get; set; }
        public int Pontos { get; set; }
        public bool Ativo { get; set; }
    }
}
