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
        var roleId = Context.User.FindFirst("RoleId").Value;
        
        _logger.LogInformation($"Подключился: {connectionId}");
        
        await Groups.AddToGroupAsync(connectionId, roleId);
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation($"Отключился: {connectionId}");

        await base.OnDisconnectedAsync(exception);
    }
}