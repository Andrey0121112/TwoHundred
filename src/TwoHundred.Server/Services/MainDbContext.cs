using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using System.Linq;

namespace TwoHundred.Server.Services;

public class MainDbContext : DbContext, IMainDbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var item in ChangeTracker.Entries<IEntity>().AsEnumerable())
        {
            item.Entity.CreatedAt = DateTime.Now;
            item.Entity.UpdatedAt = DateTime.Now;
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>().ToTable("Contract");
        modelBuilder.Entity<Company>().ToTable("Company");
        modelBuilder.Entity<ContractHistory>().ToTable("ContractHistory");
     }
}
