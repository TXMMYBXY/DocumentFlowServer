namespace DocumentFlowServer.Entities.Enums;

/// <summary>
/// Enum for notification type
/// </summary>
public enum NotificationKind
{
    TemplateAdded,
    TemplateUpdated,
    TemplateDeleted,
    DepartmentAdded,
    DepartmentUpdated,
    DepartmentDeleted,
    UserAdded,
    UserUpdated,
    UserDeleted,
    TaskAssigned,
    StatementApproved,
    System
}