using DocumentFlowServer.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Controllers.Worker;

public class WorkerAuthorizeAttribute : AuthorizeAttribute
{
    public WorkerAuthorizeAttribute()
    {
        AuthenticationSchemes = WorkerAuthenticationHandler.SchemeName;
        Roles = "Worker";
    }
}
