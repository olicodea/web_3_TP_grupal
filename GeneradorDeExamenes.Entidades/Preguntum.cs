using System;
using System.Collections.Generic;

namespace GeneradorDeExamenes.Entidades;

public partial class Preguntum
{
    public int IdPregunta { get; set; }

    public int IdExamen { get; set; }

    public string PreguntaTexto { get; set; } = null!;

    public string RespuestaUsuario { get; set; } = null!;

    public string? RespuestaCorrecta { get; set; }

    public virtual Examan IdExamenNavigation { get; set; } = null!;
}
