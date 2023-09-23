using Data.Constantes;
using System;
using System.Collections.Generic;
using System.Text;
using DateTime = System.DateTime;

namespace Data.Models
{
    public class Evento
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
        public string DataHorarioInicioFormatado { get { return DataHorarioInicio.ToString("dd/MM/yyyy hh:mm"); } }
        public int Duracao { get; set; }
        public bool Ativo { get; set; }
        public string AreaId { get; set; }
        public string DescricaoArea { get; set; }
        public string MesFormatado { get { return FormatacaMes(DataHorarioInicio); } }
        public string DiaFormatado { get { return DataHorarioInicio.Day.ToString(); } }
        private string FormatacaMes(DateTime data)
        {
            var mes = data.Month;

            switch (mes)
            {
                case 1:
                    return "JAN";
                case 2:
                    return "FEV";
                case 3:
                    return "MAR";
                case 4:
                    return "ABR";
                case 5:
                    return "MAI";
                case 6:
                    return "JUN";
                case 7:
                    return "JUL";
                case 8:
                    return "AGO";
                case 9:
                    return "SET";
                case 10:
                    return "OUT";
                case 11:
                    return "NOV";
                case 12:
                    return "DES";
                default:
                    return "--";
            }
        }
    }
}
