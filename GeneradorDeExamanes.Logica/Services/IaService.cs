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
        private static List<Examen> _examenes = new List<Examen>();
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
                string textoConPrompt = $"Generame 10 preguntas de este texto: {examen.TextoOriginal}";

                var requestBody = new
                {
                    contents = new[]
                    {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = textoConPrompt
                        }
                    }
                }
            }
                };

                var jsonResponse = await _apiService.PostAsync("v1/models/gemini-1.5-pro:generateContent", requestBody);
                var responseData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (responseData == null)
                {
                    _logger.LogError("La respuesta de la API es nula.");
                    return null;
                }

                var candidates = responseData["candidates"];
                if (candidates == null || candidates.Count == 0)
                {
                    _logger.LogError("No se encontraron candidatos en la respuesta de la API.");
                    return null;
                }

                var content = candidates[0]["content"];
                if (content == null)
                {
                    _logger.LogError("El contenido del candidato es nulo.");
                    return null;
                }

                var parts = content["parts"];
                if (parts == null || parts.Count == 0)
                {
                    _logger.LogError("No se encontraron partes en el contenido.");
                    return null;
                }

                var textPart = parts[0]["text"];
                if (textPart == null)
                {
                    _logger.LogError("El texto de la parte es nulo.");
                    return null;
                }

                var text = textPart.ToString();
                var lines = text.Split('\n');

                var questions = new List<string>();
                foreach (var line in lines)
                {
                    if (Regex.IsMatch(line, @"^\d+\.\s"))
                    {
                        questions.Add(line.Trim());
                    }
                }

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




        public async Task<Examen> GetFeedback(Examen examen)
        {
            try
            {
                var textoConRespuestas = "Quiero que me hagas una correccion de las respuestas a las preguntas que te envío, segun tu informacion hasta ahora, y que me digas correcto o incorrecto, si queres agregar una pequeña devolucion en el caso incorrecto, esta bien, de lo contrario solo limitate a poner correcto o incorrecto:\n\n";
                foreach (var pregunta in examen.Preguntas)
                {
                    textoConRespuestas += $"{pregunta.Texto}\nRespuesta: {pregunta.RespuestaUsuario}\n\n";
                }

                var requestBody = new
                {
                    contents = new[]
                    {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = textoConRespuestas
                        }
                    }
                }
            }
                };

                var jsonResponse = await _apiService.PostAsync("v1/models/gemini-1.5-pro:generateContent", requestBody);
                var responseData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (responseData != null && responseData["candidates"] != null && responseData["candidates"][0] != null
                    && responseData["candidates"][0]["content"] != null && responseData["candidates"][0]["content"]["parts"] != null
                    && responseData["candidates"][0]["content"]["parts"][0] != null && responseData["candidates"][0]["content"]["parts"][0]["text"] != null)
                {
                    var feedbackText = responseData["candidates"][0]["content"]["parts"][0]["text"].ToString();
                    examen.Feedback = feedbackText;
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




        public List<Examen> GetExamenes()
        {
            return _examenes;
        }

    }
}
