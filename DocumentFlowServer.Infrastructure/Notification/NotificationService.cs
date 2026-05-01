using System.Threading.Tasks;
using DocumentFlowServer.Application.Common.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Notification;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public NotificationService(ILogger<NotificationService> logger, IHubContext<NotificationHub> notificationHub)
    {
        _logger = logger;
        _notificationHub = notificationHub;
    }
    
    public async Task SendNotificationToAllAsync(Entities.Models.Notification notification)
    {
        _logger.LogDebug("Notification {Kind} {Severity} {Title} {Message}",
            notification.Kind, notification.Severity, notification.Title, notification.Message);
        
        await _notificationHub.Clients.All.SendAsync("Notification", notification);
    }

    public async Task SendNotificationToRoleAsync(int[] roleIds, Entities.Models.Notification notification)
    {
        _logger.LogDebug("Notification {Kind} {Severity} {Title} {Message}",
            notification.Kind, notification.Severity, notification.Title, notification.Message);
        foreach (var roleId in roleIds)
        {
            await _notificationHub.Clients.Group(roleId.ToString()).SendAsync("Notification", notification);
        }
    }

    public async Task SendNotificationToUserAsync(int userId, Entities.Models.Notification notification)
    {
        _logger.LogDebug("Notification {Kind} {Severity} {Title} {Message}",
            notification.Kind, notification.Severity, notification.Title, notification.Message);
        
        await _notificationHub.Clients.User(userId.ToString()).SendAsync("Notification", notification);
    }
}