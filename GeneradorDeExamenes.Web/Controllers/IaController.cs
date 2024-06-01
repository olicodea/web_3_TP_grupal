using GeneradorDeExamanes.Logica.Services;
using GeneradorDeExamenes.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneradorDeExamenes.Web.Controllers
{
    public class IaController : Controller
    {
        private readonly IIaService _iaService;

        public IaController(IIaService iaService)
        {
            _iaService = iaService;
        }

        public IActionResult GeneradorPreguntas()
        {
            return View(new ExamenModel());
        }

        [HttpPost]
        public async Task<IActionResult> GenerateQuestions(ExamenModel examenModel)
        {
            try
            {
                var examenEntidad = examenModel.MapearAEntidad();
                var preguntasGeneradas = await _iaService.GenerateQuestions(examenEntidad);

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

        public IActionResult MostrarPreguntas(ExamenModel examenModel)
        {
            return View(examenModel);
        }

        [HttpPost]
        public async Task<IActionResult> CorregirRespuestas(ExamenModel examenModel)
        {
            try
            {
                var examenEntidad = examenModel.MapearAEntidad();
                var examenConFeedback = await _iaService.GetFeedback(examenEntidad);

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


        public IActionResult MostrarFeedback(ExamenModel examenModel)
        {
            return View(examenModel);
        }
    }
}
