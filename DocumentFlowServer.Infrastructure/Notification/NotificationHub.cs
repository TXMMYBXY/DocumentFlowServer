using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Notification;

public class NotificationHub : Hub
{
    private ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var role = Context.User.FindFirst(ClaimTypes.Role).Value;
        
        _logger.LogInformation($"Подключился: {connectionId}");
        
        await Groups.AddToGroupAsync(connectionId, role);
        
        _logger.LogInformation("Added to group: {Role}", role);
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation($"Отключился: {connectionId}");

        await base.OnDisconnectedAsync(exception);
    }
}