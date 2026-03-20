using EventAssos.Core.Interfaces.Tools;

namespace EventAssos.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
  public string Hash(string password)
  {
    //Sel
    return BCrypt.Net.BCrypt.HashPassword(password);
  }

  public bool Verify(string password, string hashedPassword)
  {
    //verif sa correspondance entre sel et clearPassword
    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
  }
}