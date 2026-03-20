using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Core.DTOs.Response;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using EventAssos.Core.Mappers;
using EventAssos.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EventAssos.Core.Services;

public class AuthService(
  IMembreRepository membreRepository,
  IPasswordHasher passwordHasher,
  IJwtService jwtService) : IAuthService
{
  #region LoginAsync

  public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
  {
    Membre? membre = await membreRepository.GetByEmailAsync(loginDto.Email);

    if (membre == null || !passwordHasher.Verify(loginDto.Password, membre.Password))
    {
      throw new Exception("Identifiants invalides");
    }
    
    string token = jwtService.GenererToken(membre);

    return new AuthResponseDto
    {
      Token = token,
      Pseudo = membre.Pseudo,
    };
  }

  #endregion

  #region RegisterAsync

  public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto)
  {
    if (string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
    {
      throw new ArgumentException("Veuillez entrer toutes les informations requises à l'inscription");
    }

    bool emailExiste = await membreRepository.EmailExistsAsync(registerDto.Email);
    if (emailExiste)
    {
      throw new Exception("Cet email est déja utilisé");
    }

    bool pseudoExiste = await membreRepository.PseudoExistsAsync(registerDto.Pseudo);
    if (pseudoExiste)
    {
      throw new Exception("Ce pseudo est déja utilisé");
    }

    string hashedPassword = passwordHasher.Hash(registerDto.Password);
    
    Membre nouveauMembre = registerDto.ToEntity(hashedPassword);

    string token = jwtService.GenererToken(nouveauMembre);

    await membreRepository.AddAsync(nouveauMembre);

    return new AuthResponseDto
    {
      Token = token,
      Pseudo = registerDto.Pseudo,
    };
  }

  #endregion
}