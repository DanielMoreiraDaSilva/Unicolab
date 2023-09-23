using System;

namespace Data.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool Ativo { get; set; }
        public string StatusFormatado { get; set; }
        public bool PrimeiroAcesso { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public string Token { get; set; }
        public DateTime DataUltimoToken { get; set; }
        public string PerfilId { get; set; }
        public string PerfilDescricao { get; set; }
        public bool ResetSolicitado { get; set; }
        public int Pontos { get; set; }
        public string RA { get; set; }
        public bool Selecionado { get { return false; } }
        public string AreaDescricao { get; set; }
        public string CursoDescricao { get; set; }
    }
}
