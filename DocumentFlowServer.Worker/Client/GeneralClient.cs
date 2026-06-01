using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DocumentFlowServer.Worker.Configuration;
using DocumentFlowServer.Worker.Interface.Client;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Worker.Client;

public class GeneralClient : IGeneralClient
{
    private readonly HttpClient _httpClient;
    private readonly WorkerSettings _settings;

    public GeneralClient(HttpClient httpClient, IOptions<WorkerSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }
    
    public async Task<TResponse?> GetResponseAsync<TResponse>(string uri, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(_settings.ApiBaseUrl + uri, ct);

        await _IsSuccessStatusCode(response, ct);

        var responseJson = await response.Content.ReadAsStringAsync(ct);

        return _ConvertResponse<TResponse>(responseJson);
    }

    public async Task<TResponse?> PostResponseAsync<TRequest, TResponse>(TRequest request, string uri, CancellationToken ct)
    {
        var requestJson = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_settings.ApiBaseUrl + uri, requestContent, ct);
        await _IsSuccessStatusCode(response, ct);

        var responseJson = await response.Content.ReadAsStringAsync(ct);

        return _ConvertResponse<TResponse>(responseJson);
    }

    private static async Task _IsSuccessStatusCode(HttpResponseMessage response, CancellationToken ct)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            var result = new ErrorResponse();
            try
            {
                result =
                    JsonSerializer.Deserialize<ErrorResponse>(errorContent);

            }
            catch (JsonException ex)
            {
                throw new HttpRequestException(
                    $"Request failed with status {response.StatusCode}",
                    null,
                    response.StatusCode
                );
            }
            
            throw new HttpRequestException(
                $"Request failed with status {response.StatusCode}, message: {result.Message}",
                null,
                response.StatusCode
            );
        }
    }

    private static T? _ConvertResponse<T>(string response)
    {
        if (response.Equals(""))
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(response);
    }
}
