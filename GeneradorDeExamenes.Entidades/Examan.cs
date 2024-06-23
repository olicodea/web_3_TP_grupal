using System;
using System.Collections.Generic;

namespace GeneradorDeExamenes.Entidades;

public partial class Examan
{
    public int IdExamen { get; set; }

    public int? Calificacion { get; set; }

    public string Feedback { get; set; } = null!;
    public int? IdCategoria { get; set; }

    public virtual Categoria Categoria { get; set; }

    public virtual ICollection<Preguntum> Pregunta { get; set; } = new List<Preguntum>();
}
