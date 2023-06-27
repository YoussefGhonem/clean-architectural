using System.ComponentModel.DataAnnotations.Schema;

namespace Elearninig.Base.Domain.Common;

public record BaseActivityEntity
{
    protected Guid _id = Guid.NewGuid();
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get => _id; private set => _id = value; }
    public DateTimeOffset CreatedDate { get; private set; }
    public Guid? CreatedBy { get; private set; }

    public void MarkAsCreated(Guid? userId)
    {
        CreatedDate = DateTimeOffset.UtcNow;
        CreatedBy = userId;
    }
};