namespace Elearninig.Base.Application.Dtos;

public record AuditableEntityDto
{
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
    public UserInfo CreatedBy { get; set; }
    public UserInfo LastModifiedBy { get; set; }
}
