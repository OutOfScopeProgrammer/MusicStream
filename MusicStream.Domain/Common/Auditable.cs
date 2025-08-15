namespace MusicStream.Domain.Common;

public abstract class Auditable
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
