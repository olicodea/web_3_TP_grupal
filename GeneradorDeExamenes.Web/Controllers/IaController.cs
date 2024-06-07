using GeneradorDeExamanes.Logica.Services;
using GeneradorDeExamenes.Models;

using Microsoft.AspNetCore.Mvc;

namespace GeneradorDeExamenes.Web.Controllers;

public class IaController : Controller
{
    private readonly IIaService _iaService;

    public IaController(IIaService iaService)
    {
        _iaService = iaService;
    }

    public IActionResult GeneradorPreguntas()
    {
        return View(new ExamenViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> GenerateQuestions(ExamenViewModel examenModel)
    {
        try
        {
            var preguntasGeneradas = await _iaService.GenerateQuestions(examenModel);

            if (preguntasGeneradas != null && preguntasGeneradas.Any())
            {
                examenModel.Preguntas = preguntasGeneradas.Select(p =>
                {
                    var preguntaModel = new PreguntaModel();
                    preguntaModel.ParsearPregunta(p);
                    return preguntaModel;
                }).ToList();

                return View("MostrarPreguntas", examenModel);
            }
            else
            {
                ViewBag.Error = "No se pudieron generar preguntas.";
                return View("GeneradorPreguntas", examenModel);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al generar preguntas: " + ex.Message;
            return View("GeneradorPreguntas", examenModel);
        }
    }

    public IActionResult MostrarPreguntas(ExamenViewModel examenModel)
    {
        return View(examenModel);
    }

    [HttpPost]
    public async Task<IActionResult> CorregirRespuestas(ExamenViewModel examenModel)
    {
        try
        {
            var examenConFeedback = await _iaService.GetFeedback(examenModel);

            if (examenConFeedback != null && !string.IsNullOrEmpty(examenConFeedback.Feedback))
            {
                examenModel.Feedback = examenConFeedback.Feedback;
                return View("MostrarFeedback", examenModel);
            }
            else
            {
                ViewBag.Error = "No se pudo obtener el feedback.";
                return View("MostrarPreguntas", examenModel);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al obtener el feedback: " + ex.Message;
            return View("MostrarPreguntas", examenModel);
        }
    }


    public IActionResult MostrarFeedback(ExamenViewModel examenModel)
    {
        return View(examenModel);
    }
}
