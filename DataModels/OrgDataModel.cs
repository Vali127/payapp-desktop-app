using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using PayApp.Data;

namespace PayApp.DataModels;

public class OrgDataModel
{
    private readonly string? _dbconnectionsetting = Environment.GetEnvironmentVariable("DB_CONNECTION_SETTING"); // Env var is charged from program.cs

    public ObservableCollection<Department> GetDepartments()
    {
        var departments = new ObservableCollection<Department>();

        using var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        string sql = "SELECT * FROM DEPARTEMENT";
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var dept = new Department
            {
                IdDepartement = reader["id_departement"].ToString()!,
                NomDepartement = reader["nom_departement"].ToString()!,
                Description = reader["description_departement"].ToString()!
            };

            departments.Add(dept);
        }

        return departments;
    }
}