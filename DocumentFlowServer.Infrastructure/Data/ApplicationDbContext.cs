using DocumentFlowServer.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }
    public DbSet<Entities.Models.Document> Documents { get; set; }
    public DbSet<Entities.Models.AboutUserModels.User> Users { get; set; }
    public DbSet<Entities.Models.Template> Templates { get; set; }
    public DbSet<Entities.Models.AboutUserModels.RefreshToken> RefreshTokens { get; set; }
    public DbSet<IssueModel> Tasks { get; set; }
    public DbSet<LoginHistory> LoginHistories { get; set; }
    public DbSet<Entities.Models.Department> Departments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}