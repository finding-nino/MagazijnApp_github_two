namespace MagazijnApp.Services;

public class UserSession
{
    public string? CurrentUsername { get; set; }
    public string? CurrentRole { get; set; }
    public bool IsAdmin => CurrentRole == "admin";
}