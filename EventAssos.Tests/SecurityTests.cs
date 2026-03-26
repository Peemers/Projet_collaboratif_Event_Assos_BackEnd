using EventAssos.Infrastructure.Security; // Vérifie ton namespace
using Xunit;
using Xunit.Abstractions; // Pour afficher du texte dans la console de test

namespace EventAssos.Tests;

public class SecurityTests(ITestOutputHelper output)
{
  [Fact]
  public void GenerateAdminHash()
  {
    // Arrange
    var passwordHasher = new PasswordHasher();
    string passwordToHash = "Test1234@";

    // Act
    string hash = passwordHasher.Hash(passwordToHash);

    // Assert - On affiche le résultat dans la console du test
    output.WriteLine($"Voici ton Hash stable pour le Seeding : {hash}");
        
    Assert.NotEmpty(hash);
  }
}