namespace API.DTOs;

public class UserDto
{
    public required string Id { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string DisplayName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public required string Token { get; set; } = string.Empty;
}
