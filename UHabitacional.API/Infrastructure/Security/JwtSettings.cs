namespace UHabitacional.API.Infrastructure.Security;

/// <summary>
/// Configuración de JWT cargada desde appsettings.json.
/// </summary>
public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 120;
}
