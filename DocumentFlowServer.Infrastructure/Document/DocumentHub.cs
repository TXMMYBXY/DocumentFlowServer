using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Document;

public class DocumentHub : Hub
{
    private ILogger<DocumentHub> _logger;

    public DocumentHub(ILogger<DocumentHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        
        _logger.LogInformation($"Подключился: {connectionId}");
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation($"Отключился: {connectionId}");

        await base.OnDisconnectedAsync(exception);
    }
}