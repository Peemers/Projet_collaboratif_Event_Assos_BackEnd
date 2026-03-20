using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

namespace EventAssos.Core.Mappers;

public static class MembreMapper
{
  public static Membre ToEntity(this RegisterRequestDto dto, string hashedPassword)
  {
    return new Membre
    {
      Id = Guid.NewGuid(),
      Email = dto.Email,
      Pseudo = dto.Pseudo,
      Password = hashedPassword,
      Genre = dto.Genre,
      DateNaissance = (DateTime)dto.DateNaissance!,
      DateInscription = DateTime.UtcNow,
      Role = Roles.Membre
    };
  }
}