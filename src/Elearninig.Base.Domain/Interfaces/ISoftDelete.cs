namespace Elearninig.Base.Domain.Interfaces;

public interface ISoftDelete
{
    public bool IsDeleted { get; }
    public DateTimeOffset? DeletedDate { get; }
    public Guid? DeletedBy { get; }
    public void MarkAsDeleted(Guid? userId);
    public void MarkAsNotDeleted();
}