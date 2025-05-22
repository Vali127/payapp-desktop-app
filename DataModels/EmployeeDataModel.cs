using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using PayApp.Data;

namespace PayApp.DataModels;

public class EmployeeDataModel
{
    private readonly string? _dbConnectionString=Environment.GetEnvironmentVariable("DB_CONNECTION_SETTING");

    public ObservableCollection<Employee> GetEmployees()
    {
        var employees = new ObservableCollection<Employee>();
        
        using var connection = new MySqlConnection(_dbConnectionString);
        connection.Open();
        
        string query = "SELECT * FROM EMPLOYE";
        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var employee = new Employee
            {
                IdEmploye = reader["id_employe"].ToString()!,
                NomEmploye = reader["nom_employe"].ToString()!,
                PrenomEmploye = reader["prenom_employe"].ToString()!,
                DateNaissance = reader["datenais"] is DBNull 
                    ? "N/A" 
                    : Convert.ToDateTime(reader["datenais"]).ToString("MM/dd/yyyy"),
                Email = reader["email"].ToString()!,
                NumTelephone = reader["telephone"].ToString()!,
                Sexe = reader["sexe"].ToString()!,
            };
            employees.Add(employee);
        }

        return employees;


    }

    public void InsertEmployee(string nom, string prenom, string sexe, DateTime dateNaissance, string email, string telephone)
    {
        using var connection = new MySqlConnection(_dbConnectionString);
        connection.Open();

        string query = @"INSERT INTO EMPLOYE(nom_employe, prenom_employe, datenais, email, telephone, sexe) 
                     VALUES (@nom, @prenom, @datenais, @email, @telephone, @sexe)";

        using var command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@nom", nom);
        command.Parameters.AddWithValue("@prenom", prenom);
        command.Parameters.AddWithValue("@datenais", dateNaissance);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@telephone", telephone);
        command.Parameters.AddWithValue("@sexe", sexe);

        command.ExecuteNonQuery();
    }

}