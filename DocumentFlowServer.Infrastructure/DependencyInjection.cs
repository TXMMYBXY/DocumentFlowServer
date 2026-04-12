using DocumentFlowServer.Application.Repository;
using DocumentFlowServer.Application.Repository.Account;
using DocumentFlowServer.Application.Repository.Department;
using DocumentFlowServer.Application.Repository.Role;
using DocumentFlowServer.Application.Repository.Task;
using DocumentFlowServer.Application.Repository.Template;
using DocumentFlowServer.Application.Repository.Token;
using DocumentFlowServer.Application.Repository.User;
using DocumentFlowServer.Application.Services;
using DocumentFlowServer.Application.Services.Authorization;
using DocumentFlowServer.Application.Services.Department;
using DocumentFlowServer.Application.Services.FieldExtractor;
using DocumentFlowServer.Application.Services.FileStorage;
using DocumentFlowServer.Application.Services.Notification;
using DocumentFlowServer.Application.Services.Personal;
using DocumentFlowServer.Application.Services.RefreshTokenHasher;
using DocumentFlowServer.Application.Services.Role;
using DocumentFlowServer.Application.Services.Tasks;
using DocumentFlowServer.Application.Services.Template;
using DocumentFlowServer.Application.Services.User;
using DocumentFlowServer.Application.Services.WorkerTask;
using DocumentFlowServer.Infrastructure.Configuration;
using DocumentFlowServer.Infrastructure.Data;
using DocumentFlowServer.Infrastructure.Repository;
using DocumentFlowServer.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentFlowServer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IWorkerTaskService, WorkerTaskService>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IFieldExtractorService, FieldExtractorService>();
        services.AddScoped<IPersonalAccountService, PersonalAccountService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<DataSeeder>();
        
        services.Configure<RefreshTokenSettings>(configuration.GetSection(nameof(RefreshTokenSettings)));
        services.Configure<WorkerSettings>(configuration.GetSection(nameof(WorkerSettings)));
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseLazyLoadingProxies()
                .UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21)),
                    options => options.EnableRetryOnFailure());
        });

        return services;
    }
}