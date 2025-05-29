using System;
using MySql.Data.MySqlClient;

namespace PayApp.DataModels;

public struct PdfGeneratorData
{
    public string? NameEmp;
    public string? Firstname;
    public string? Post;
    public string? Department;
    public string? Salary;
}

public class ServicesDataModel
{ 
    // Env var is charged from program.cs
    public static PdfGeneratorData  GetPdfGeneratorData(string id)
    {
        var dbconnectionsetting = Environment.GetEnvironmentVariable("DB_CONNECTION_SETTING");
        PdfGeneratorData result = default;
        var connection = new MySqlConnection(dbconnectionsetting);
        connection.Open();

        var sql = "SELECT e.nom_employe AS nom, e.prenom_employe AS prenom, p.nom_poste AS poste, d.nom_departement AS departement, s.salaire_base AS salaire, s.impot AS impot, (s.salaire_base - s.impot) AS salaire_net FROM EMPLOYE e JOIN POSTE p ON e.id_poste = p.id_poste JOIN DEPARTEMENT d ON p.id_departement = d.id_departement JOIN SALAIRE s ON p.id_poste = s.id_poste WHERE e.id_employe = @id;";
        
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            result.NameEmp = reader["nom"].ToString();
            result.Firstname = reader["prenom"].ToString();
            result.Post = reader["poste"].ToString();
            result.Department = reader["departement"].ToString();
            result.Salary = reader["salaire_net"].ToString();
        }
        
        return result;
    }
}