using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Exceptions;
using UHabitacional.API.Infrastructure.Security;

namespace UHabitacional.API.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ISesionService _sesionService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUsuarioRepository usuarioRepo, ISesionService sesionService, JwtSettings jwtSettings)
    {
        _usuarioRepo = usuarioRepo;
        _sesionService = sesionService;
        _jwtSettings = jwtSettings;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto, string? direccionIP, string? userAgent, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.GetByEmailAsync(dto.Email, ct)
            ?? throw new ForbiddenOperationException("Credenciales inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            throw new ForbiddenOperationException("Credenciales inválidas.");

        var rol = usuario.TipoUsuario?.Nombre ?? "Desconocido";

        // JTI único por sesión, así podemos invalidarla en BD
        var jti = Guid.NewGuid().ToString("N");
        var expira = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellidos}"),
            new Claim(ClaimTypes.Role, rol),
            new Claim("IdTipoUsuario", usuario.IdTipoUsuario.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expira,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Crear y persistir la sesión en BD
        await _sesionService.CrearAsync(
            usuario.IdUsuario, jti, expira, direccionIP, userAgent, ct);

        return new LoginResponseDto(
            usuario.IdUsuario,
            usuario.Email,
            $"{usuario.Nombre} {usuario.Apellidos}",
            rol,
            tokenString,
            expira);
    }
}
