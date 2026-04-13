using DocumentFlowServer.Application.Department.Dtos;
using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Application.User.Dtos;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.User;

public class UserRepository : BaseRepository<Entities.Models.AboutUserModels.User>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<UserDto>> GetAllUsers(UserFilter filter)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(u => u.Email.Contains(filter.Email));

        if (!string.IsNullOrWhiteSpace(filter.FullName))
            query = query.Where(u => u.FullName.Contains(filter.FullName));

        if (filter.DepartmentId.HasValue)
            query = query.Where(u => u.DepartmentId == filter.DepartmentId);

        if (filter.RoleId.HasValue)
            query = query.Where(u => u.RoleId == filter.RoleId);

        var reusltQuery = query
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Department = new DepartmentDto
                {
                    Id = u.DepartmentId,
                    Description = u.Department.Description
                },
                IsActive = u.IsActive,
                Role = new RoleDto
                {
                    Id = u.RoleId,
                    Description = u.Role.Description
                }
            });

        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            reusltQuery = reusltQuery
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }

        return await reusltQuery.ToListAsync();
    }

    public async Task<bool> ChangeUserStatusAsync(int userId)
    {
        await _dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(x => x.IsActive, x => !x.IsActive)
            );

        return await _dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => u.IsActive)
            .FirstAsync();
    }

    public async Task SetNewPasswordAsync(int userId, string hash)
    {
        await _dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(x => x.PasswordHash, x => hash)
            );
    }
}