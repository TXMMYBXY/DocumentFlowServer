namespace DocumentFlowServer.Entities.Enums;

/// <summary>
/// Enum for notification type
/// </summary>
public enum NotificationKind
{
    TemplateAdded,
    TemplateUpdated,
    UserAdded,
    UserDeleted,
    TaskAssigned,
    StatementApproved,
    System
}