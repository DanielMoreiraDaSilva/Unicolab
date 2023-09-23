using System.Collections.Generic;

namespace Business.TransferObjects
{
    public class PerfilDto
    {
        public string Id { get; set; }
        public string Descricao { get; set; }

        public List<ModuloDto> Modulos { get; set; }
    }
}
