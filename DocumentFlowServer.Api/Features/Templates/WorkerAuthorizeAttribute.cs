using DocumentFlowServer.Infrastructure.Common.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Features.Templates;

public class WorkerAuthorizeAttribute : AuthorizeAttribute
{
    public WorkerAuthorizeAttribute()
    {
        AuthenticationSchemes = WorkerAuthenticationHandler.SchemeName;
        Roles = "Worker";
    }
}