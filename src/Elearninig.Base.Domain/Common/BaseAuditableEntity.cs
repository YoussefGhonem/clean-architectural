namespace Elearninig.Base.Domain.Common
{
    public record BaseAuditableEntity : BaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public Guid? LastModifiedBy { get; set; }
    }
}
