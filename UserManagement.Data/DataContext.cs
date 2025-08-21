using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : IdentityDbContext<User>, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<Log>()
            .HasOne(l => l.User)
            .WithMany(u => u.Logs)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Log>? Logs { get; set; }

    public TEntity? Find<TEntity>(object id) where TEntity : class
        => base.Find<TEntity>(id);

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }

    public async Task<TEntity?> FindAsync<TEntity>(object id) where TEntity : class
        => await base.FindAsync<TEntity>(id);

    public Task<IQueryable<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        => Task.FromResult(base.Set<TEntity>().AsQueryable());

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        await SaveChangesAsync();
    }
}
