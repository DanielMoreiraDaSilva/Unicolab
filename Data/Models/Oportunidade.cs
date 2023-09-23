using System;
using System.Globalization;

namespace Data.Models
{
    public class Oportunidade
    {
        public string Id { get; set; }
        public string CursoId { get; set; }
        public string CursoDescricao { get; set; }
        public string UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public string Titulo { get; set; }
        public string Empresa { get; set; }
        public string Descricao { get; set; }
        public decimal Salario { get; set; }
        public string SalarioFormatado { get { return FormatacaoSalario(Salario); } }
        public DateTime DataInicio { get; set; }
        public string DataInicioFormatada { get { return DataInicio.ToString("dd/MM/yyyy"); } }
        public DateTime? DataFim { get; set; }
        public bool Ativo { get; set; }
        private string FormatacaoSalario(decimal valor)
        {
            string formatoPreco = valor < 1000 ? string.Concat("{0:0.00}") : string.Concat("{0:0,0.00}");
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "R$ " + formatoPreco, valor);
        }
    }
}
