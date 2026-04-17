using System.Text;
using DocumentFlowServer.Application.Account;
using DocumentFlowServer.Application.Common.Configuration;
using DocumentFlowServer.Application.Common.MappingProfiles;
using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.Department;
using DocumentFlowServer.Application.FieldExtractor;
using DocumentFlowServer.Application.Issue;
using DocumentFlowServer.Application.Jwt;
using DocumentFlowServer.Application.Personal;
using DocumentFlowServer.Application.RefreshToken;
using DocumentFlowServer.Application.Role;
using DocumentFlowServer.Application.Template;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Infrastructure.Account;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Common.Services;
using DocumentFlowServer.Infrastructure.Data;
using DocumentFlowServer.Infrastructure.Department;
using DocumentFlowServer.Infrastructure.Issue;
using DocumentFlowServer.Infrastructure.Jwt;
using DocumentFlowServer.Infrastructure.RefreshToken;
using DocumentFlowServer.Infrastructure.Role;
using DocumentFlowServer.Infrastructure.Template;
using DocumentFlowServer.Infrastructure.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DocumentFlowServer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<RefreshTokenSettings>(configuration.GetSection(nameof(RefreshTokenSettings)));
        
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);
        services.AddAutoMapper(typeof(DepartmentMappingProfile).Assembly);
        services.AddAutoMapper(typeof(RefreshTokenMappingProfile).Assembly);
        services.AddAutoMapper(typeof(RoleMappingProfile).Assembly);
        services.AddAutoMapper(typeof(IssueMappingProfile).Assembly);
        services.AddAutoMapper(typeof(LoginTimeMappingProfile).Assembly);

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped(typeof(ITemplateRepository<>), typeof(TemplateRepository<>));
        services.AddScoped<IIssueRepository, IssueRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IFieldExtractorService, FieldExtractorService>();
        services.AddScoped(typeof(ITemplateService<>), typeof(TemplateService<>));
        services.AddScoped<IIssueService, IssueService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPersonalAccountService, PersonalAccountService>();

        services.AddScoped<DataSeeder>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseLazyLoadingProxies()
                .UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21)),
                    options => options.EnableRetryOnFailure());
        });
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis:Configuration").Value;
            options.InstanceName = configuration.GetSection("Redis:InstanceName").Value;
        });
        
        var authSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        services
        .AddAuthorization()
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;  // Для разработки (в продакшене должно быть true)
            options.SaveToken = true;              // Сохраняем токен в контексте
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,                        // Проверяем подпись
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(authSettings.SecretKey)),   // Ключ для проверки
                ValidateIssuer = true,                                  // Проверяем издателя
                ValidIssuer = authSettings.Issuer,                      // Валидный издатель
                ValidateAudience = true,                                // Проверяем аудиторию
                ValidAudience = authSettings.Audience,                  // Валидная аудитория
                ValidateLifetime = true,                                // Проверяем время жизни
                ClockSkew = TimeSpan.Zero                               // Не даем дополнительного времени
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notifications"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
        
        return services;
    }
}