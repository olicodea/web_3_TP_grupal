using GeneradorDeExamenes.Entidades;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneradorDeExamanes.Logica.Services
{
    public interface IIaService
    {
        Task<string[]> GenerateQuestions(Examen examen);
        Task<Examen> GetFeedback(Examen examen);
        public List<Examen> GetExamenes();

    }
    public class IaService : IIaService
    {
        private static readonly List<Examen> _examenes = new List<Examen>();
        private readonly IApiService _apiService;
        private readonly ILogger<IaService> _logger;

        public IaService(IApiService apiService, ILogger<IaService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<string[]> GenerateQuestions(Examen examen)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[] { new { parts = new[] { new { text = $"Generame 10 preguntas de este texto: {examen.TextoOriginal}" } } } }
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

        public async Task<Examen> GetFeedback(Examen examen)
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

        private string BuildFeedbackText(Examen examen)
        {
            var textoConRespuestas = "Quiero que me hagas una correccion de las respuestas a las preguntas que te envío, segun tu informacion hasta ahora, y que me digas correcto o incorrecto, si queres agregar una pequeña devolucion en el caso incorrecto, esta bien, de lo contrario solo limitate a poner correcto o incorrecto:\n\n";
            foreach (var pregunta in examen.Preguntas)
            {
                textoConRespuestas += $"{pregunta.Texto}\nRespuesta: {pregunta.RespuestaUsuario}\n\n";
            }
            return textoConRespuestas;
        }

        public List<Examen> GetExamenes()
        {
            return _examenes;
        }

    }
}
