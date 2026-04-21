using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ForgottenEmpire.Controllers;

public class AuthenticationRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthenticationController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate(AuthenticationRequest request)
    {
        // 🔒 SEGURIDAD: Credenciales ahora obtenidas de configuración segura (user-secrets)
        // En producción, esto debe buscar en base de datos con hashing (bcrypt, Argon2)
        string userRole = "";

        // Validar credenciales de admin desde configuración segura
        var adminUsername = _config["Authentication:AdminUsername"];
        var adminPassword = _config["Authentication:AdminPassword"];

        if (request.Username == adminUsername && request.Password == adminPassword)
        {
            userRole = "Admin";
        }
        else
        {
            // Validar credenciales de usuario desde configuración segura
            var userUsername = _config["Authentication:UserUsername"];
            var userPassword = _config["Authentication:UserPassword"];

            if (request.Username == userUsername && request.Password == userPassword)
            {
                userRole = "User";
            }
            else
            {
                return Unauthorized("Credenciales inválidas");
            }
        }

        // ⚠️  NOTA DE SEGURIDAD: Esta implementación es temporal para desarrollo.
        // En producción implementar:
        // 1. Base de datos con usuarios hasheados
        // 2. Rate limiting para prevenir ataques de fuerza bruta
        // 3. Logging de intentos fallidos
        // 4. Bloqueo temporal después de múltiples fallos

        // L�gica de Generaci�n de Token
        //Prococinamos la Signature
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]!)); // Se usa para acceder al appsettings.json
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //Algoritmo de cifrado

        var claims = new List<Claim> //
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim("role", userRole)
            // Rol ventaja en claim =>
            // Preguntar si el usuario tiene X rol sin ir a la BD = > M�s r�pido y mas cash.
        };

        //Token para el usuario
        var jwtSecurityToekn = new JwtSecurityToken( //Objeto que representa el token y datos appsettings.json
            issuer: _config["Authentication:Issuer"],
            audience: _config["Authentication:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signingCredentials);

        string TokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToekn);
        return Ok(TokenToReturn); //Llave
    }
}