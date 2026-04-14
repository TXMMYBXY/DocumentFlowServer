using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Common.Services;

public interface INotificationService
{
    Task SendNotificationToAllAsync(Notification notification);
    Task SendNotificationToUserAsync(string userId, Notification notification);
    Task SendNotificationToRoleAsync(string[] roleIds, Notification notification);
}