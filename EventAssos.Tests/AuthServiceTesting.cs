using Castle.Core.Configuration;
using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using EventAssos.Core.Services;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using Microsoft.Extensions.Logging;
using NSubstitute;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;


namespace EventAssos.Tests;

public class AuthServiceTesting
{
  private readonly IMembreRepository _membreRepository = Substitute.For<IMembreRepository>();
  private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
  private readonly IJwtService _jwtService = Substitute.For<IJwtService>();
  private readonly IEmailService _emailService = Substitute.For<IEmailService>();
  private readonly IConfiguration _config = Substitute.For<IConfiguration>();
  private readonly ILogger<AuthService> _logger = Substitute.For<ILogger<AuthService>>();

  private readonly AuthService _authService;
  
  public AuthServiceTesting()
  {
    _authService = new AuthService(_membreRepository, _passwordHasher, _jwtService, _emailService, _config, _logger );
  }
  [Fact]
  public async Task LoginAsync_ReturnToken_LoginOk()
  {
    LoginRequestDto loginDto = new LoginRequestDto { Email = "test@test.com", Password = "Password123!" };
    Membre membre = new Membre {Email = "test@test.com", Password = "Password123!", Pseudo = "Tester", Genre = Genres.Homme, Role =  Roles.Membre};
    
    _membreRepository.GetByEmailAsync(loginDto.Email).Returns(membre);
    _passwordHasher.Verify(loginDto.Password, membre.Password).Returns(true);
    _jwtService.GenererToken(membre).Returns("fake-jwt-token");

    var result = await _authService.LoginAsync(loginDto);
    
    Assert.Equal("fake-jwt-token", result.Token);
    Assert.Equal("Tester", result.Pseudo);
  }

  [Fact]
  public async Task LoginAsync_PasswordIncorrect_ReturnsError()
  {
    LoginRequestDto loginDto = new LoginRequestDto { Email = "test@test.be", Password = "WrongPassword" };
    Membre membre = new Membre{Email = "test@test.com", Password = "HashedPassword", Pseudo = "Tester", Genre =  Genres.Homme, Role = Roles.Membre};
    
    _membreRepository.GetByEmailAsync(loginDto.Email).Returns(membre);
    _passwordHasher.Verify(loginDto.Password, membre.Password).Returns(false);
    
    await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(loginDto));
    _jwtService.DidNotReceive().GenererToken(Arg.Any<Membre>());
  }

  [Fact]
  public async Task RegisterAsync_ReturnToken_RegisterOk()
  {
    RegisterRequestDto registerDto = new RegisterRequestDto{
      Pseudo = "test", 
      Email = "testunitaire@test.be",  
      Password = "Password123!", 
      Genre = Genres.Homme, 
      DateNaissance = new DateTime(1986, 09, 16)
    };
    Membre membre = new Membre{Email = "testunitaire@test.be",  Password = "Password123!", Pseudo = "Tester2", Genre = Genres.Homme, Role = Roles.Membre};
    
    _membreRepository.GetByEmailAsync(registerDto.Email).Returns((Membre?)null);
    _passwordHasher.Hash(registerDto.Password).Returns("HashedPassword123!");
    _jwtService.GenererToken(Arg.Any<Membre>()).Returns("fake-jwt-token");
    
    var result = await _authService.RegisterAsync(registerDto);
    
    Assert.Equal("fake-jwt-token", result.Token);
    Assert.Equal("test", result.Pseudo);
    
    await _membreRepository.Received(1).AddAsync(Arg.Any<Membre>());
  }
}