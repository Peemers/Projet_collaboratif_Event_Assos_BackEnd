using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Core.DTOs.Response;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using EventAssos.Core.Mappers;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAssos.Core.Services;

public class AuthService(
  IMembreRepository membreRepository,
  IPasswordHasher passwordHasher,
  IJwtService jwtService,
  IEmailService emailService,
  IConfiguration config,
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
    
    log.LogInformation("Connexion réussie de {Email}, token attribué", loginDto.Email);
    return new AuthResponseDto
    {
      Token = token,
      Pseudo = membre.Pseudo,
      Role = membre.Role,
      Id = membre.Id
    };
  }

  #endregion

  #region RegisterAsync

  public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto)
  {
    log.LogInformation($"Tentative d'enregistrement avec : {registerDto.Email}", registerDto);
    if (string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
    {
      //log.LogInformation($"Inscription de : {registerDto.Email} échouée par manque d'informations requises", registerDto);
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

    await membreRepository.AddAsync(nouveauMembre);
    
    log.LogInformation($"Attribution token et connexion de : {registerDto.Email} - {registerDto.Pseudo}");
    string token = jwtService.GenererToken(nouveauMembre);

    return new AuthResponseDto
    {
      Token = token,
      Pseudo = nouveauMembre.Pseudo,
      Role = nouveauMembre.Role,
      Id = nouveauMembre.Id,
    };
  }

  #endregion
  
  #region ResetPasswordAsync

  public async Task ResetPasswordAsync(string email)
  {
    Membre? membres = await  membreRepository.GetByEmailAsync(email);
    if (membres == null) return;
    
    string token = jwtService.GenererToken(membres);
    
    string? baseUrl = config["AppSettings:ClientUrl"];
    string lien = $"{baseUrl}/reset-password?token={token}&email={email}";

    string message = $@"<h2>Reinitialisation de mot de passe</h2>
                        <p>Bonjour {membres.Pseudo}</p>
                        <p>Cliquez sur le lien pour réinitialiser votre mot de passe
                        <a href='{lien}' style='padding: 10px; background: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Changer mon mot de passe</a>
                        <p>Ce lien expirera dans 2 heures.</p>";

    await emailService.EnvoyerMailAsync(email, "Changement de mot de passe - Event'Assos", message);
    log.LogInformation("Email de réinitialisation envoyé à {email}", email);
  }
  
  
  #endregion
}