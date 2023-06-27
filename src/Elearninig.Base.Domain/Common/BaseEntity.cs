using Elearninig.Base.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearninig.Base.Domain.Common;

public record BaseEntity : ISoftDelete
{
    protected Guid _id = Guid.NewGuid();
    [DatabaseGenerated(DatabaseGeneratedOption.None)] //this indicates that the property value is not generated by the database.
    public Guid Id { get => _id; private set => _id = value; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedDate { get; private set; }
    public Guid? DeletedBy { get; private set; }

    // domain event
    public readonly List<BaseEvent> _domainEvents = new();
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void MarkAsDeleted(Guid? userId)
    {
        IsDeleted = true;
        DeletedDate = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }

    public void MarkAsNotDeleted()
    {
        IsDeleted = false;
        DeletedBy = null;

    }
    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}