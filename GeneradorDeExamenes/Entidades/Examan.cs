using System;
using System.Collections.Generic;

namespace GeneradorDeExamenes.Web;

public partial class Examan
{
    public int IdExamen { get; set; }

    public int? Calificacion { get; set; }

    public string Feedback { get; set; } = null!;

    public virtual ICollection<Preguntum> Pregunta { get; set; } = new List<Preguntum>();
}
