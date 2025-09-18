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
        // Lógica de Validación de Usuario (Hardcodeada)
        // En una app real, esto buscaría en la base de datos (min 27 clase 10).
        string userRole = "";
        if (request.Username == "admin" && request.Password == "adminpassword")
        {
            userRole = "Admin";
        }
        else if (request.Username == "RamonAyala" && request.Password == "Moncho")
        {
            userRole = "User";
        }
        else
        {
            return Unauthorized();
        }

        //Si es por entidad seria esta la logica * cre* porque en la clase no se dice:
        // llamas a repositoy para buscar el usuario, pasando username y password[DromBody],
        // si lo encuentra devuelve un obj User, si no lo encuentra devuelve null

        // Lógica de Generación de Token
        //Prococinamos la Signature
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]!)); // Se usa para acceder al appsettings.json
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //Algoritmo de cifrado

        var claims = new List<Claim> //
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim("role", userRole)
            // Rol ventaja en claim =>
            // Preguntar si el usuario tiene X rol sin ir a la BD = > Más rápido y mas cash.
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