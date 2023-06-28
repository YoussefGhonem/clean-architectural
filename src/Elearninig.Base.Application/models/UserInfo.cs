namespace Elearninig.Base.Application.models;

public class UserInfo
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? ImageUrl { get; set; }
}
public class UserData
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

public class UserInfoFromDb
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? ImageId { get; set; }
    public string? ImageUrl { get; set; }
    public bool? ImageIsExternal { get; set; }
}