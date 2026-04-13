using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Entities.Models.AboutUserModels;
using DocumentFlowServer.Entities.Models.DocumentTemplatesModels;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Entities.Models.AboutUserModels.User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ContractTemplate> ContractTemplates { get; set; }
    public DbSet<StatementTemplate> StatementTemplates { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<LoginHistory> LoginHistories { get; set; }
    public DbSet<Entities.Models.Department> Departments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}