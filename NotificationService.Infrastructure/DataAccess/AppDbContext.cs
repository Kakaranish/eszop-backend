using Common.Domain.Repositories;
using Common.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NotificationService.Domain.Aggregates.NotificationAggregate;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Notification> Notifications { get; private set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Notification>()
                .Property(x => x.Metadata)
                .HasConversion(
                    x => x == null ? null : JsonConvert.SerializeObject(x),
                    x => x == null ? null : JsonConvert.DeserializeObject<IDictionary<string, string>>(x));
        }

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            return await this.SaveChangesWithRetriesAsync(cancellationToken);
        }
    }
}
