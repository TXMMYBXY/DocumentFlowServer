using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFlowServer.Application.Department.Dtos;
using DocumentFlowServer.Application.Personal.Dtos;
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

    public async Task<ICollection<UserDto>> GetAllUsersAsync(UserFilter filter)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .AsQueryable();
        
        if(filter.SortBy.HasValue)
        {
            query = filter.SortBy.Value switch
            {
                UserSortField.Email =>
                    filter.Descending
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email),

                UserSortField.FullName =>
                    filter.Descending
                        ? query.OrderBy(u => u.FullName)
                        : query.OrderByDescending(u => u.FullName),

                UserSortField.Role =>
                    filter.Descending
                        ? query.OrderBy(u => u.Role)
                        : query.OrderByDescending(u => u.Role),

                UserSortField.Department =>
                    filter.Descending
                        ? query.OrderBy(u => u.Department)
                        : query.OrderByDescending(u => u.Department),

                UserSortField.IsActive =>
                    filter.Descending
                        ? query.OrderBy(u => u.IsActive)
                        : query.OrderByDescending(u => u.IsActive),

                _ =>
                    filter.Descending
                        ? query.OrderBy(u => u.Role)
                        : query.OrderByDescending(u => u.Role)
            };
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(u => u.Email.Contains(filter.Email));

        if (!string.IsNullOrWhiteSpace(filter.FullName))
            query = query.Where(u => u.FullName.Contains(filter.FullName));

        if (filter.DepartmentId.HasValue)
            query = query.Where(u => u.DepartmentId == filter.DepartmentId);

        if (filter.Role.HasValue)
            query = query.Where(u => u.Role == filter.Role);

        var reusltQuery = query
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Department = new DepartmentCleanDto
                {
                    Id = u.DepartmentId,
                    Title = u.Department.Title,
                    Description = u.Department.Description
                },
                IsActive = u.IsActive,
                Role = u.Role
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

    public async Task<UserLoginDto?> GetUserForLoginAsync(string email)
    {
        return await _dbContext.Users
            .Where(u => u.Email.Equals(email))
            .Select(u => new UserLoginDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                PasswordHash = u.PasswordHash,
                IsActive = u.IsActive,
                Department = new DepartmentCleanDto
                {
                    Id = u.DepartmentId,
                    Title = u.Department.Title,
                    Description = u.Department.Description
                },
                Role = u.Role
            })
            .SingleOrDefaultAsync();
    }

    public async Task<UserLoginDto?> GetUserForAccessAsync(int userId)
    {
        return await _dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserLoginDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                PasswordHash = u.PasswordHash,
                IsActive = u.IsActive,
                Department = new DepartmentCleanDto
                {
                    Id = u.DepartmentId,
                    Title = u.Department.Title,
                    Description = u.Department.Description
                },
                Role = u.Role
            })
            .SingleOrDefaultAsync();
    }

    public async Task<PersonDto?> GetCurrentUserByIdAsync(int userId)
    {
        return await _dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => new PersonDto
            {
                FullName = u.FullName,
                Email = u.Email,
                Department = u.Department.Title,
                Role = u.Role
            })
            .SingleOrDefaultAsync();
    }
}