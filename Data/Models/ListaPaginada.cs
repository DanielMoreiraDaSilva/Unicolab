using System.Collections.Generic;

namespace Data.Models
{
    public class ListaPaginada<T>
    {
        public List<T> Lista { get; set; }
        public int Paginas { get; set; }
        public int TotalItens { get; set; }
    }
}
