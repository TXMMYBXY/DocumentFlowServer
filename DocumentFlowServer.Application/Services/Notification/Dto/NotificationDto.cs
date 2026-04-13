using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Services.Notification.Dto;

public sealed record NotificationDto
(
    NotificationKind Kind,
    NotificationSeverity Severity,
    string Title,
    string Message
);