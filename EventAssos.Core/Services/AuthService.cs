using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Core.DTOs.Response;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using EventAssos.Core.Mappers;
using EventAssos.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAssos.Core.Services;

public class AuthService(
  IMembreRepository membreRepository,
  IPasswordHasher passwordHasher,
  IJwtService jwtService,
  ILogger<AuthService> log) : IAuthService
{
  #region LoginAsync

  public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
  {
    log.LogInformation($"Tentative de connexion de {loginDto.Email}", loginDto);
    
    Membre? membre = await membreRepository.GetByEmailAsync(loginDto.Email);

    if (membre == null || !passwordHasher.Verify(loginDto.Password, membre.Password))
    {
      log.LogInformation("Connexion échouée : Mauvais identifiant");
      throw new Exception("Identifiants invalides");
    }
    
    string token = jwtService.GenererToken(membre);
    
    log.LogInformation($"Connexion réussie de {loginDto.Email}, token attribué");
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
    log.LogInformation($"Tentative d'enregistrement avec : {registerDto.Email}", registerDto);
    if (string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
    {
      throw new ArgumentException("Veuillez entrer toutes les informations requises à l'inscription");
    }

    bool emailExiste = await membreRepository.EmailExistsAsync(registerDto.Email);
    if (emailExiste)
    {
      log.LogInformation($"Inscription échouée de : {registerDto.Email} : Email existe déja en DB", registerDto);
      throw new Exception("Cet email est déja utilisé");
    }

    bool pseudoExiste = await membreRepository.PseudoExistsAsync(registerDto.Pseudo);
    if (pseudoExiste)
    {
      log.LogInformation($"Inscription échouée de : {registerDto.Email} avec pseudo : {registerDto.Pseudo} : Pseudo existe déja en DB", registerDto);
      throw new Exception("Ce pseudo est déja utilisé");
    }
    
    log.LogInformation($"Inscription de {registerDto.Email} avec pseudo : {registerDto.Pseudo} réussie", registerDto);
    string hashedPassword = passwordHasher.Hash(registerDto.Password);
    
    Membre nouveauMembre = registerDto.ToEntity(hashedPassword);

    
    log.LogInformation($"Attribution token et connexion de : {registerDto.Email} - {registerDto.Pseudo}");
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