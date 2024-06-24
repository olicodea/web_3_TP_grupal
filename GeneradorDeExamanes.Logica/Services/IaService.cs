using GeneradorDeExamenes.Entidades;
using GeneradorDeExamenes.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneradorDeExamanes.Logica.Services;

public interface IIaService
{
    Task<string[]> GenerateQuestions(ExamenViewModel examen);
    Task<ExamenViewModel> GetFeedback(ExamenViewModel examen);
    public List<ExamenViewModel> GetExamenes();
    Task<string> ClasificarCategoriaAsync(string texto);

    Task<string> CalificarExamenAsync(string texto);
}
public class IaService : IIaService
{
    private static readonly List<ExamenViewModel> _examenes = new List<ExamenViewModel>();
    private readonly IApiService _apiService;
    private readonly ILogger<IaService> _logger;

    public IaService(IApiService apiService, ILogger<IaService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<string[]> GenerateQuestions(ExamenViewModel examen)
    {
        try
        {
            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = $"Generame 10 preguntas de este texto: {examen.Resumen}" } } } }
            };

            var jsonResponse = await _apiService.PostAsync("v1/models/gemini-1.5-pro:generateContent", requestBody);
            var responseData = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            if (responseData == null || responseData.Candidates == null || responseData.Candidates.Count == 0)
            {
                _logger.LogError("No se encontraron candidatos en la respuesta de la API.");
                return null;
            }

            var questions = ExtractQuestions(responseData.Candidates[0].Content.Parts);
            if (questions.Any())
            {
                _examenes.Add(examen);
                return questions.ToArray();
            }
            else
            {
                _logger.LogError("No se encontraron preguntas en el texto.");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Excepción al llamar a la API: {ex.Message}");
            return null;
        }
    }

    private List<string> ExtractQuestions(List<Part> parts)
    {
        var questions = new List<string>();
        foreach (var part in parts)
        {
            var text = part.Text;
            var lines = text.Split('\n');
            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, @"^\d+\.\s"))
                {
                    questions.Add(line.Trim());
                }
            }
        }
        return questions;
    }

    public async Task<ExamenViewModel> GetFeedback(ExamenViewModel examen)
    {
        try
        {
            var textoConRespuestas = BuildFeedbackText(examen);

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = textoConRespuestas } } } }
            };

            var jsonResponse = await _apiService.PostAsync("v1/models/gemini-1.5-pro:generateContent", requestBody);
            var responseData = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            if (responseData != null && responseData.Candidates != null && responseData.Candidates.Count > 0 &&
                responseData.Candidates[0].Content != null && responseData.Candidates[0].Content.Parts != null)
            {
                examen.Feedback = responseData.Candidates[0].Content.Parts[0].Text;

                return examen;
            }
            else
            {
                _logger.LogError("La estructura de la respuesta de la API no es la esperada.");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Excepción al llamar a la API: {ex.Message}");
            return null;
        }
    }

    private string BuildFeedbackText(ExamenViewModel examen)
    {
        var textoConRespuestas = "Quiero que me hagas una correccion de las respuestas a las preguntas que te envío, segun tu informacion hasta ahora, y que me digas correcto o incorrecto, si queres agregar una pequeña devolucion en el caso incorrecto, esta bien, de lo contrario solo limitate a poner correcto o incorrecto:\n\n";
        foreach (var preguntaModel in examen.Preguntas)
        {
            textoConRespuestas += $"{preguntaModel.PreguntaTexto}\nRespuesta: {preguntaModel.RespuestaUsuario}\n\n";
        }
        return textoConRespuestas;
    }

    public List<ExamenViewModel> GetExamenes()
    {
        return _examenes;
    }

    public async Task<string> ClasificarCategoriaAsync(string texto)
    {
        try
        {
            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = $"Quiero que me digas a que rama pertenece este texto, solo quiero que elijas 1, solo dime la palabra, nada mas: Arte, Naturales, Informática, Sociales, Economía. {texto}" } } } }
            };

            var jsonResponse = await _apiService.PostAsync("v1/models/gemini-1.5-pro:generateContent", requestBody);
            var responseData = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            if (responseData == null || responseData.Candidates == null || responseData.Candidates.Count == 0)
            {
                _logger.LogError("No se encontraron candidatos en la respuesta de la API.");
                return null;
            }

            string categoria = responseData.Candidates[0].Content.Parts[0].Text;
           
            string categoriaAsignada = MapCategoria(categoria);

            return categoriaAsignada;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Excepción al llamar al servicio de clasificación: {ex.Message}");
            return null;
        }
    }

    private string MapCategoria(string categoriaObtenida)
    {


        if (categoriaObtenida.Contains("arte", StringComparison.OrdinalIgnoreCase))
            return "Arte";
        else if (categoriaObtenida.Contains("naturales", StringComparison.OrdinalIgnoreCase))
            return "Naturales";
        else if (categoriaObtenida.Contains("informática", StringComparison.OrdinalIgnoreCase))
            return "Informática";
        else if (categoriaObtenida.Contains("sociales", StringComparison.OrdinalIgnoreCase))
            return "Sociales";
        else if (categoriaObtenida.Contains("economía", StringComparison.OrdinalIgnoreCase))
            return "Economía";
        else
            return "Otra"; 
    }

    public async Task<string> CalificarExamenAsync(string texto)
    {
        try
        {
            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = $"Quiero que me des una calificación para el examen teniendo en cuenta el feedback que diste sobre el mismo, solo tenes que decir un numero entre 1 y 10 nada mas. Tene en cuenta que no es necesario que las respuestas hayan sido perfectas, con que sean correctas en gran parte es suficiente para considerar el punto, como son 10 preguntas, vale 1 punto cada una. Recorda solo decir un numero entre 1 y 10 nada mas.\n {texto}" } } } }
            };

            var jsonResponse = await _apiService.PostAsync("v1/models/gemini-1.5-pro:generateContent", requestBody);
            var responseData = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            if (responseData == null || responseData.Candidates == null || responseData.Candidates.Count == 0)
            {
                _logger.LogError("No se encontraron candidatos en la respuesta de la API.");
                return null;
            }

            return responseData.Candidates[0].Content.Parts[0].Text;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Excepción al llamar al servicio de clasificación: {ex.Message}");
            return null;
        }
    }

}
