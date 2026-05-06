using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Entities.Enums;
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
                Role = Role.Administrator,
                DepartmentId = 1,
                IsActive = true
            };

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.SaveChangesAsync();
        }
    }
}