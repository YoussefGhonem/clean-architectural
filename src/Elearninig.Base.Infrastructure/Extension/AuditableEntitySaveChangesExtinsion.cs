using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Elearninig.Base.Infrastructure.Extension;
public static class AuditableEntitySaveChangesExtinsion
{
    //  you can easily determine if an entity entry has any owned entities that have been changed.
    //  This can be useful when you need to handle scenarios specific to owned entities,
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() && // checks if the target entry represents an owned entity.
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
