using DocumentFlowServer.Application.Services;
using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Infrastructure.Data;

public class DataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasherService _passwordHasher;

    public DataSeeder(ApplicationDbContext dbContext, IPasswordHasherService passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (!_dbContext.Roles.Any())
        {
            var roles = new Role[4]
            {
                new Role {Title = "Admin"},
                new Role {Title = "Boss"},
                new Role {Title = "Purchaser"},
                new Role {Title = "Employee"},
            };

            await _dbContext.Roles.AddRangeAsync(roles);
            await _dbContext.SaveChangesAsync();
        }

        if (!_dbContext.Departments.Any())
        {
            var itDep = new Department
            {
                Title = "Информационный отдел"
            };

            await _dbContext.Departments.AddAsync(itDep);
            await _dbContext.SaveChangesAsync();
        }
        
        if (!_dbContext.Users.Any())
        {
            var admin = new User
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