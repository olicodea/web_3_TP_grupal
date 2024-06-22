using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorDeExamenes.Entidades
{
    public partial class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Examan> Examenes { get; set; } = new List<Examan>();
    }
}
