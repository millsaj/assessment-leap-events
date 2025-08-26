namespace Api.Domain.Entities;

public class Event
{
    public virtual Guid Id { get; set; }
    public virtual string Name { get; set; } = string.Empty;
    public virtual DateTime StartsOn { get; set; }
    public virtual DateTime EndsOn { get; set; }
    public virtual string Location { get; set; } = string.Empty;
}
