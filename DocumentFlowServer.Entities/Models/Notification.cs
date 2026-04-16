using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Entities.Models;

/// <summary>
/// Notification record
/// </summary>
/// <param name="Kind">Enum of notification type</param>
/// <param name="Severity">Enum of notification category</param>
/// <param name="Title">Notification title</param>
/// <param name="Message">Message</param>
public sealed record Notification
(
    NotificationKind Kind,
    NotificationSeverity Severity,
    string Title,
    string Message
);