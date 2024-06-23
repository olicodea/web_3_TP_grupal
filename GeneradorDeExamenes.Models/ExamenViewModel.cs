using GeneradorDeExamenes.Entidades;

namespace GeneradorDeExamenes.Models;

public class ExamenViewModel
{
    public string Resumen { get; set; }
    public string Feedback { get; set; }
    public int? IdCategoria { get; set; }
    public string CategoriaNombre { get; set; }
    public List<PreguntaModel> Preguntas { get; set; } = new List<PreguntaModel>();

    public ExamenViewModel() { }

    public ExamenViewModel(Examan examen)
    {
        Preguntas = PreguntaModel.ParsearLista(examen.Pregunta);
        Feedback = examen.Feedback;
        IdCategoria = examen.IdCategoria;
        CategoriaNombre = examen.Categoria?.Nombre;
    }

    public Examan MapearAEntidad()
    {
        return new Examan
        {
            Pregunta = PreguntaModel.ParsearListaAEntidad(Preguntas),
            Feedback = Feedback,
            IdCategoria = IdCategoria
        };
    }

    public static List<ExamenViewModel> MapearLista(List<Examan> examenes)
    {
        return examenes.ConvertAll(e => new ExamenViewModel(e));
    }
}

public class PreguntaModel
{
    public string PreguntaTexto { get; set; }
    public string RespuestaUsuario { get; set; }
    public string RespuestaCorrecta { get; set; }

    public static List<PreguntaModel> ParsearLista(ICollection<Preguntum> preguntas)
    {
        var listaPreguntas = new List<PreguntaModel>();
        foreach (var pregunta in preguntas)
        {
            var preguntaModel = new PreguntaModel();
            preguntaModel.ParsearPregunta(pregunta.PreguntaTexto);
            listaPreguntas.Add(preguntaModel);
        }
        return listaPreguntas;
    }

    public void ParsearPregunta(string pregunta)
    {
        var partes = pregunta.Split('.');
        if (partes.Length > 1)
        {
            PreguntaTexto = partes[1].Trim();
        }
        else
        {
            PreguntaTexto = pregunta.Trim();
        }
    }

    public Preguntum MapearAEntidad()
    {
        return new Preguntum
        {
            PreguntaTexto = PreguntaTexto,
            RespuestaUsuario = RespuestaUsuario
        };
    }

    public static List<Preguntum> ParsearListaAEntidad(List<PreguntaModel> preguntas)
    {
        return preguntas.ConvertAll(p => p.MapearAEntidad());
    }
}