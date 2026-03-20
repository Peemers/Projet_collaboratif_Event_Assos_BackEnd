using EventAssos.Domain.Entities;

namespace EventAssos.Core.Interfaces.Tools;

public interface IJwtService
{
  //entité membre complete pour remplir le token de ce qu'on veut
  string GenererToken(Membre membre);
}