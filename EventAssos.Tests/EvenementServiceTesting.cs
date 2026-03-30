using Castle.Core.Logging;
using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using EventAssos.Core.Services;
using EventAssos.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace EventAssos.Tests;

public class EvenementServiceTesting
{
  private readonly ICategorieRepository _categorieRepository = Substitute.For<ICategorieRepository>();
  private readonly IEvenementRepository _evenementRepository = Substitute.For<IEvenementRepository>();
  private readonly ILogger<EvenementService> _logger = Substitute.For<ILogger<EvenementService>>();
  
  private readonly EvenementService _evenementService;

  public EvenementServiceTesting()
  {
    _evenementService = new EvenementService(_evenementRepository, _categorieRepository, _logger);
  }
  
  [Fact]
  public async Task CreateAsync_ThrowException_CategorieNotFound()
  {
    EvenementRequestDto dto = new EvenementRequestDto
    {
      Nom = "Festival",
      CategorieIds = new List<int> { 999 },
      Description = "Description Test",
      DateDebut = new DateTime(2026, 04, 01),
      DateFin = new DateTime(2026, 04, 02),
      DateLimiteInscription =  new DateTime(2026, 03, 30),
      NbMax = 5,
      NbMin = 1,
      Lieu = "Charleroi",
      ListeAttenteActive = false
    };

    _categorieRepository.GetByIdAsync(999).Returns((Categorie?)null);
    
    var exception = await Assert.ThrowsAsync<Exception>(() => _evenementService.CreateAsync(dto));
    
    Assert.Equal("Événement non créé - Categorie invalide", exception.Message);
    await _evenementRepository.DidNotReceive().AddAsync(Arg.Any<Evenement>());
  }
}