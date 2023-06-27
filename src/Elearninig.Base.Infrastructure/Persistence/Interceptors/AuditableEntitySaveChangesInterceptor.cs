using Elearninig.Base.Domain.Common;
using Elearninig.Base.Infrastructure.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Elearninig.Base.Infrastructure.Persistence.Interceptors;
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    public void ChangeTrackerEntity(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.Now;

            }
            else if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedDate = DateTime.Now;

            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.Entity.LastModifiedDate = DateTime.Now;
            }
        }

    }
}
