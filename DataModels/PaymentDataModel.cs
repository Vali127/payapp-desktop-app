using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using PayApp.Data;

namespace PayApp.DataModels;

public class PaymentDataModel
{
    private readonly string? _dbconnectionsetting = Environment.GetEnvironmentVariable("DB_CONNECTION_SETTING"); // Env var is charged from program.cs

    public ObservableCollection<Payment> GetPayedEmployee()
    {
        var result = new ObservableCollection<Payment>();
        var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        var sql = "SELECT e.id_employe, e.nom_employe, p.nom_poste FROM PAIEMENT pa JOIN EMPLOYE e ON pa.id_employe = e.id_employe JOIN POSTE p ON e.id_poste = p.id_poste;";
        
        var cmd = new MySqlCommand(sql, connection);
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var tmp = new Payment
            {
                Id = reader["id_employe"].ToString()!,
                Nom = reader["nom_employe"].ToString()!,
                NomPoste = reader["nom_poste"].ToString()!
            };
            result.Add(tmp);
        }
        
        return result;
    }
}