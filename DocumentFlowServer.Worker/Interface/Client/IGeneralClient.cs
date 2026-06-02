using System.Threading;
using System.Threading.Tasks;

namespace DocumentFlowServer.Worker.Interface.Client;

public interface IGeneralClient
{
    /// <summary>
    /// Базовый метод для эндпоинтов Post
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="uri">эндпоинт</param>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <returns>TResponse</returns>
    Task<TResponse?> PostResponseAsync<TRequest, TResponse>(TRequest request, string uri, CancellationToken ct);
    
    /// <summary>
    /// Базовый метод для эндпоинтов Get
    /// </summary>
    /// <param name="uri">эндпоинт</param>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <returns>TResponse</returns>
    Task<TResponse?> GetResponseAsync<TResponse>(string uri, CancellationToken ct);
}
