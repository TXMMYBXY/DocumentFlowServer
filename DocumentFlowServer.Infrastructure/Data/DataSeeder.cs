using DocumentFlowServer.Application.Common.Services;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Data;

public class DataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public DataSeeder(ApplicationDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (!await _dbContext.Roles.AnyAsync())
        {
            var roles = new Entities.Models.AboutUserModels.Role[4]
            {
                new Entities.Models.AboutUserModels.Role {Title = "Admin"},
                new Entities.Models.AboutUserModels.Role {Title = "Boss"},
                new Entities.Models.AboutUserModels.Role {Title = "Purchaser"},
                new Entities.Models.AboutUserModels.Role {Title = "Employee"},
            };

            await _dbContext.Roles.AddRangeAsync(roles);
            await _dbContext.SaveChangesAsync();
        }

        if (!await _dbContext.Departments.AnyAsync())
        {
            var itDep = new Entities.Models.Department
            {
                Title = "Информационный отдел"
            };

            await _dbContext.Departments.AddAsync(itDep);
            await _dbContext.SaveChangesAsync();
        }
        
        if (! await _dbContext.Users.AnyAsync())
        {
            var admin = new Entities.Models.AboutUserModels.User
            {
                FullName = "Admin",
                Email = "admin@test.com",
                PasswordHash = _passwordHasher.Hash("1234"),
                RoleId = 1,
                DepartmentId = 1,
                IsActive = true
            };

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.SaveChangesAsync();
        }
    }
}