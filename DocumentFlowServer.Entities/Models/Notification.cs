using DocumentFlowAPI.Enums;

namespace DocumentFlowServer.Entities.Models;

public sealed record Notification
(
    NotificationKind Kind,
    NotificationSeverity Severity,
    string Title,
    string Message
);