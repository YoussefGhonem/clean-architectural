namespace TicketManagement.Base.Helpers.Models;

public class FileDto
{
    public string Id { get; set; }
    public string Url { get; set; }
    public bool IsExternal { get; set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }

    public FileDto()
    {
    }

    public FileDto(string id, string url, bool isExternal)
    {
        Id = id ?? Guid.NewGuid().ToString();
        Url = url;
        IsExternal = isExternal;
    }
}