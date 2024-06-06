using GeneradorDeExamanes.Configurations;
using Newtonsoft.Json;
using System.Text;
using GeneradorDeExamanes.Logica.Utils;

namespace GeneradorDeExamanes.Logica.Services;

public interface IApiService
{
    Task<string> PostAsync(string endpoint, object data);
}
public class ApiService :IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly KeyDecoder _keyDecoder;

        public ApiService(HttpClient httpClient, ApiSettings apiSettings, KeyDecoder keyDecoder)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
            _keyDecoder = keyDecoder;
        }

    public async Task<string> PostAsync(string endpoint, object data)
    {

            try
            {
                var requestContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiSettings.BaseUrl}/{endpoint}?key={_apiSettings.ApiKey}", requestContent);
                var requestContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiSettings.BaseUrl}/{endpoint}?key={apiKey}", requestContent);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException httpRequestException)
        {
            // Manejo específico de excepciones HTTP
            throw new Exception($"Error al realizar la solicitud HTTP: {httpRequestException.Message}", httpRequestException);
        }
        catch (Exception ex)
        {
            // Manejo general de excepciones
            throw new Exception($"Error al realizar la solicitud: {ex.Message}", ex);
        }

        }

    }

}
