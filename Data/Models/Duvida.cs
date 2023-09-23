using System;

namespace Data.Models
{
    public class Duvida
    {
        public string Id { get; set; }
        public string Pergunta { get; set; }
        public int Pontos { get; set; }
        public DateTime DataHora {get;set;}
        public string UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string MateriaId { get; set; }
        public string NomeMateria { get; set; }
        public int QuantidadeResposta { get; set; }
    }
}