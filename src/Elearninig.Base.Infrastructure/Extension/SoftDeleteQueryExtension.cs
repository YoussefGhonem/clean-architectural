
using Elearninig.Base.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Elearninig.Base.Infrastructure.Extension;

public static class SoftDeleteQueryExtension
{
    public static void GetOnlyNotDeletedEntities(this ModelBuilder builder)
    {
        // It loops through all the entity types in the model (builder.Model.GetEntityTypes())
        // and checks if each entity type implements the ISoftDelete interface
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            // ClrType is used to retrieve the CLR (Common Language Runtime) type of an entity or entity type
            // ( When we refer to the "CLR type," we are referring to the actual underlying type of an object or entity in the .NET runtime environment.).
            // ClrType is typically used within the Entity Framework Core infrastructure to perform operations related to entity types,
            // such as reflection, metadata extraction, and query generation.
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddSoftDeleteQueryFilter();
            }
        }
    }

    #region helper methods
    // 'IMutableEntityType' represents the metadata of an entity type in Entity Framework Core.
    private static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
    {
        // This method uses reflection (GetMethod, MakeGenericMethod, Invoke) to dynamically call the GetSoftDeleteFilter method
        // with the entity type's CLR type (entityData.ClrType) as the generic argument.
        // The returned filter is then applied as a query filter using entityData.SetQueryFilter.

        var methodToCall = typeof(SoftDeleteQueryExtension)?.GetMethod(nameof(GetSoftDeleteFilter),
            BindingFlags.NonPublic | BindingFlags.Static)?.MakeGenericMethod(entityData.ClrType);

        var filter = methodToCall?.Invoke(null, Array.Empty<object>());

        if (filter == null) return;
        entityData.SetQueryFilter((LambdaExpression)filter);

        entityData.AddIndex(entityData.FindProperty(nameof(ISoftDelete.IsDeleted)) ??
                            throw new InvalidOperationException());
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : class, ISoftDelete
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
    #endregion

}
