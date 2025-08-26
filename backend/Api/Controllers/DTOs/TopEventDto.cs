namespace Api.Controllers.DTOs;

public class TopEventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }
}
