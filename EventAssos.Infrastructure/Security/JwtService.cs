using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventAssos.Core.Interfaces.Tools;
using EventAssos.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EventAssos.Infrastructure.Security;

public class JwtService(IConfiguration configuration) : IJwtService
{
  public string GenererToken(Membre membre)
  {
    //Claims, infos contenue dans le token
    Claim[] claims =
    [
      new Claim(JwtRegisteredClaimNames.Sub, membre.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, membre.Email),
      new Claim(JwtRegisteredClaimNames.Nickname, membre.Pseudo),
      new Claim(ClaimTypes.Role, membre.Role.ToString())
    ];
    
    //recup clé secrete dans appsetting
    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
    
    //signature
    SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    //creation
    JwtSecurityToken token = new JwtSecurityToken(
      issuer: configuration["Jwt:Issuer"],
      audience: configuration["Jwt:Audience"],
      claims: claims,
      expires: DateTime.UtcNow.AddHours(2),
      signingCredentials: creds
    );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}