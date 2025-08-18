using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Music : Auditable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public Singer Singer { get; set; } = new();
    public Guid SingerId { get; set; }

}
