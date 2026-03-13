using EventAssos.Domain.Enums;

namespace EventAssos.Domain.Entities;

public class Evenement
{
  public Guid id { get; set; }
  public required string Description { get; set; }
  public string Lieu  { get; set; }
  public DateTime DateDebut { get; set; }
  public DateTime DateFin { get; set; }
  public int NbMin { get; set; }
  public int NbMax { get; set; }
  public StatutEvenement StatutEvenement { get; set; }
  public DateTime DateLimiteInscription { get; set; }
  public DateTime DateDeCreation { get; set; }
  public DateTime DateMaj { get; set; }
}


