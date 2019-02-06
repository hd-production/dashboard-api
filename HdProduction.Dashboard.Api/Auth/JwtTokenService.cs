using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace HdProduction.Dashboard.Api.Auth
{
  public class JwtTokenService : ISessionTokenService
  {
    private readonly JsonWebKey _privateSecret;

    public JwtTokenService(string privateKeyPath)
    {
      _privateSecret = JsonWebKey.Create(ReadFile(privateKeyPath));
    }

    public string CreateToken(User user)
    {
      // authentication successful so generate jwt token
      var tokenHandler = new JwtSecurityTokenHandler();
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
        {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Email, user.Email)
        }),
        Expires = DateTime.UtcNow.AddMinutes(30),
        Issuer = JwtDefaults.Issuer,
        SigningCredentials = new SigningCredentials(_privateSecret, SecurityAlgorithms.RsaSha256)
      };
      var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    private string ReadFile(string path)
    {
      using (var stream = File.OpenText(path))
      {
        return stream.ReadToEnd();
      }
    }
  }
}