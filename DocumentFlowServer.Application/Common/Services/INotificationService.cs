using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Common.Services;

public interface INotificationService
{
    Task SendNotificationToAllAsync(Notification notification);
    Task SendNotificationToUserAsync(int userId, Notification notification);
    Task SendNotificationToRoleAsync(int[] roleIds, Notification notification);
}