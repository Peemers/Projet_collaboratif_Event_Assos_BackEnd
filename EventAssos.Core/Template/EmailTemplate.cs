namespace EventAssos.Core.Template;

public static class EmailTemplate
{
  public static string MailNotifEvent(string pseudo, string titre, string description, string date)
  {
    return
      $@"<div style='font-family: Arial, sans-serif; color: #333;'>
         <h1 style='color: #007bff;'> Nouvel événement : {titre}</h1>
         <p>Bonjour <strong>{pseudo}</strong>,</p>
         <p>Une nouvelle activité vient d'être ajoutée sur Event'Assos :</p>
         <div style='border-left: 4px solid #007bff; padding-left: 15px; margin: 20px 0;'>
         <p><strong>Quoi :</strong> {titre}</p>
         <p><strong>Quand :</strong> {date}</p>
         <p><strong>Description :</strong> {description}</p>
         </div>
         <p>À bientôt sur Event - Assos !</p>
         </div>";
  }
  
  public static string ConfirmationInscription(string pseudo, string titreEvent, bool estEnAttente)
  {
    string statut = estEnAttente 
      ? "Vous êtes sur <strong>liste d'attente</strong>." 
      : "Votre inscription est <strong>confirmée</strong>.";

    return $@"
        <div style='font-family: Arial, sans-serif;'>
            <h1>Bonjour {pseudo} !</h1>
            <p>Vous venez de vous inscrire à l'événement : <strong>{titreEvent}</strong>.</p>
            <p>Statut : {statut}</p>
            <p>À bientôt sur Event'Assos !</p>
        </div>";
  }
}