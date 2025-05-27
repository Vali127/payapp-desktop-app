using System;
using MySql.Data.MySqlClient;

namespace PayApp.DataModels;

public class SettingPageDataModel
{
    
    public static string RestorePayment(string newDate)
    {
        // Conversion de la string en DateTime
        DateTime dateParsed = DateTime.ParseExact(newDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string mysqlDate = dateParsed.ToString("yyyy-MM-dd");

        // Connexion à la base
        string? dbconnectionsetting = Environment.GetEnvironmentVariable("DB_CONNECTION_SETTING");
        using var connection = new MySqlConnection(dbconnectionsetting);
        connection.Open();

        // Récupération de l'ancienne date
        var cmdSelect = new MySqlCommand("SELECT param_valeur FROM PARAMETRE WHERE param_nom = 'date_du_jour'", connection);
        var obj = cmdSelect.ExecuteScalar();
        DateTime oldDate = (DateTime)obj;
        
        if (dateParsed.Year < oldDate.Year || (dateParsed.Year == oldDate.Year && dateParsed.Month <= oldDate.Month))
            return "Erreur : cette date est déjà dépasser!!";
        // Mise à jour de la date
        var cmdUpdate = new MySqlCommand("UPDATE PARAMETRE SET param_valeur = @newDate WHERE param_nom = 'date_du_jour'", connection);
        cmdUpdate.Parameters.AddWithValue("@newDate", mysqlDate);
        cmdUpdate.ExecuteNonQuery();

        //Suppression des paiements
        var cmdDelete = new MySqlCommand("DELETE FROM PAIEMENT", connection);
        cmdDelete.ExecuteNonQuery();
        return "Operation executé avec succèes!!.";
    }
    
    
    public static DateTimeOffset? GetSelectedDateFromDatabase()
    {
        string? dbconnectionsetting = Environment.GetEnvironmentVariable("DB_CONNECTION_SETTING");
        using var connection = new MySqlConnection(dbconnectionsetting);
        connection.Open();

        var cmd = new MySqlCommand("SELECT param_valeur FROM PARAMETRE WHERE param_nom = 'date_du_jour'", connection);
        var obj = cmd.ExecuteScalar();

        if (obj == null || obj == DBNull.Value)
            return null;

        DateTime dateFromDb = (DateTime)obj;

        return new DateTimeOffset(dateFromDb);
    }

}