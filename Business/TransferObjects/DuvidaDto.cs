﻿using Data.Models;
using System;
using System.Collections.Generic;

namespace Business.TransferObjects
{
    public class DuvidaDto
    {
        public string Id { get; set; }
        public string Pergunta { get; set; }
        public int Pontos { get; set; }
        public DateTime? DataHora { get; set; }
        public string DataHoraFormatada { get { return DataHora.Value.ToString("dd/MM/yyyy"); } }
        public string UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string MateriaId { get; set; }
        public string NomeMateria { get; set; }
        public int QuantidadeResposta { get; set; }
        public List<Resposta> Respostas { get; set; }
    }
}