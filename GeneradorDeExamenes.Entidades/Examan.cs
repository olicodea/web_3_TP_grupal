using System;
using System.Collections.Generic;

namespace GeneradorDeExamenes.Entidades;

public partial class Examan
{
    public string TextoOriginal { get; set; }
    public int IdExamen { get; set; }

    public int Calificacion { get; set; }

    public string? Feedback { get; set; }

    public virtual ICollection<Preguntum> Pregunta { get; set; } = new List<Preguntum>();
}
