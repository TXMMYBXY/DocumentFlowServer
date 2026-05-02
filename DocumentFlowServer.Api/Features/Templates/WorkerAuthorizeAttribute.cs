using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Features.Templates;

public class WorkerAuthorizeAttribute : AuthorizeAttribute
{
    public WorkerAuthorizeAttribute()
    {
        AuthenticationSchemes = Middleware.WorkerAuthenticationHandler.SchemeName;
        Roles = "Worker";
    }
}