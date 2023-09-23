using System;
using System.Collections.Generic;
using System.Text;

namespace Business.TransferObjects
{
    public class UsuarioCursoDto
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string CursoId { get; set; }
        public bool? Ativo { get; set; }
    }
}
