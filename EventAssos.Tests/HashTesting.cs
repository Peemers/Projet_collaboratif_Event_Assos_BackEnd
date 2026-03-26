using EventAssos.Infrastructure.Security; 
using Xunit;
using Xunit.Abstractions; 

namespace EventAssos.Tests;

public class HashTesting(ITestOutputHelper output)
{
  [Fact]
  public void GenerateAdminHash()
  {
    
    var passwordHasher = new PasswordHasher();
    string passwordToHash = "Test1234@";
    
    string hash = passwordHasher.Hash(passwordToHash);
    
    output.WriteLine($"Hash : {hash}");
        
    Assert.NotEmpty(hash);
  }
}