using DocumentFlowServer.Application.Repository.Department.Dto;
using DocumentFlowServer.Application.Repository.Role.Dto;
using DocumentFlowServer.Application.Repository.User;
using DocumentFlowServer.Application.Repository.User.Dto;
using DocumentFlowServer.Application.Services.User;
using DocumentFlowServer.Entities.Data;
using DocumentFlowServer.Entities.Models.AboutUserModels;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto> GetUserByLoginAsync(string login)
    {
        return await _dbContext.Users
            .Where(x => x.Email.Equals(login))
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Department = new DepartmentDto
                {
                    Id = u.Department.Id,
                    Title = u.Department.Title
                },
                IsActive = u.IsActive,
                Role = new RoleDto
                {
                    Id = u.Role.Id,
                    Title = u.Role.Title
                }
            })
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Проверка на существующий логин
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Возвращает true, если логин уже используется</returns>
    public async Task<bool> IsUserAlreadyExists(string email)
    {
        return await _dbContext.Users.AnyAsync(x => x.Email.Equals(email));
    }

    public async Task<List<UserDto>> GetAllUsersAsync(UserFilter filter)
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
                    Id = u.Department.Id,
                    Title = u.Department.Title
                },
                IsActive = u.IsActive,
                Role = new RoleDto
                {
                    Id = u.Role.Id,
                    Title = u.Role.Title
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

    public async Task<bool> UpdateUserStatusAsync(int userId)
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

    public async Task<PersonDto> GetPersonalInfo(int personId)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => u.Id == personId)
            .Select(u => new PersonDto
            {
                FullName = u.FullName,
                Email = u.Email,
                Department = u.Department.Title,
                Role = new RoleDto
                {
                    Id = u.Role.Id,
                    Title = u.Role.Title
                },
            })
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _dbContext.Users.CountAsync();
    }

    public async Task DeleteManyAsync(List<int> ids)
    {
        var users = await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

        _dbContext.Users.RemoveRange(users);
    }

    public async Task<UserInfoDto> GetUserInfoByIdAsync(int userId)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Department = u.Department.Title,
                Role = u.Role.Title,
            })
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
}