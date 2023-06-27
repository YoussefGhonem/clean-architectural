using Elearninig.Base.Infrastructure.Extension;
using Elearninig.Base.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Elearninig.Base.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(DbContextOptions options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    #region override methods
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection.
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB. This makes
        // a single transaction including side effects from the domain event
        // handlers that are using the same DbContext with Scope lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB. This makes
        // multiple transactions. You will need to handle eventual consistency and
        // compensatory actions in case of failures.
        await _mediator.DispatchDomainEventsAsync(this);

        // After this line runs, all the changes (from the Command Handler and Domain
        // event handlers) performed through the DbContext will be committed
        return await base.SaveChangesAsync(cancellationToken);
    }
    // Within OnModelCreating, you can define entity configurations, relationships, indexes,
    // table mappings, and other aspects of the database schema, apply Fluent API configurations or data annotations to the entities in your model.
    // used for defining the model configuration and mapping entities to the database schema. 
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // to read configuration from any Infrastructure 
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        // to get not deleted in query
        builder.GetOnlyNotDeletedEntities();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
    #endregion

}
