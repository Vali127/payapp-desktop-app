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

        if (string.IsNullOrEmpty(_dbconnectionsetting))
            return result;

        using var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        var sql = "SELECT e.id_employe, e.nom_employe, p.nom_poste FROM PAIEMENT pa JOIN EMPLOYE e ON pa.id_employe = e.id_employe JOIN POSTE p ON e.id_poste = p.id_poste WHERE pa.etat = 'payé';";

        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

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

    public long GetSumOfPayedMoney()
    {
        if (string.IsNullOrEmpty(_dbconnectionsetting))
            return 0;

        using var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        var sql = "SELECT SUM(s.salaire_base - s.impot) AS total_salaire_net FROM EMPLOYE e JOIN PAIEMENT p ON e.id_employe = p.id_employe JOIN SALAIRE s ON e.id_poste = s.id_poste WHERE p.etat = 'payé';";

        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        if (reader.Read() && reader["total_salaire_net"] != DBNull.Value)
        {
            return Convert.ToInt64(reader["total_salaire_net"]);
        }
        return 0;
    }

    public long GetSumOfPayedEmployee()
    {
        if (string.IsNullOrEmpty(_dbconnectionsetting))
            return 0;

        using var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        var sql = "SELECT COUNT(*) AS nb_employe FROM PAIEMENT WHERE etat = 'payé';";

        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return Convert.ToInt64(reader["nb_employe"]);
        }
        return 0;
    }
}
