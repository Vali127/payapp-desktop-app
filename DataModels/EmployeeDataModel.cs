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
    //ajouter employe
    public void InsertEmployee(string nom, string prenom, string sexe, DateTime dateNaissance, string email, string telephone,string idposte)
    {
        using var connection = new MySqlConnection(_dbConnectionString);
        connection.Open();
        string query = @"INSERT INTO EMPLOYE(nom_employe, prenom_employe, datenais, email, telephone, sexe,id_poste) 
                     VALUES (@nom, @prenom, @datenais, @email, @telephone, @sexe,@idposte)";

        using var command = new MySqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@idposte", idposte);
        command.Parameters.AddWithValue("@nom", nom);
        command.Parameters.AddWithValue("@prenom", prenom);
        command.Parameters.AddWithValue("@datenais", dateNaissance);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@telephone", telephone);
        command.Parameters.AddWithValue("@sexe", sexe);

        try
        {
            command.ExecuteNonQuery();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erreur MySQL : " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur générale : " + e.Message);
        }
    }
    //modifier employe
   public void UpdateEmployee(string id, string nom, string prenom, string sexe, DateTime? dateNaissance, string email, string telephone)
     {
         using var connection = new MySqlConnection(_dbConnectionString);
         connection.Open();
     
         string query1 = @"SELECT nom_employe, prenom_employe, sexe, datenais, email, telephone 
                           FROM EMPLOYE WHERE id_employe = @id";
         using var command1 = new MySqlCommand(query1, connection);
         command1.Parameters.AddWithValue("@id", id);
     
         using (var reader = command1.ExecuteReader())
         {
             if (reader.Read())
             {
                 nom = string.IsNullOrWhiteSpace(nom) ? reader["nom_employe"].ToString()! : nom;
                 prenom = string.IsNullOrWhiteSpace(prenom) ? reader["prenom_employe"].ToString()! : prenom;
                 sexe = string.IsNullOrWhiteSpace(sexe) ? reader["sexe"].ToString()! : sexe;
                 dateNaissance = dateNaissance == null || dateNaissance.Value == default
                     ? reader.GetDateTime("datenais")
                     : dateNaissance;
                 email = string.IsNullOrWhiteSpace(email) ? reader["email"].ToString()! : email;
                 telephone = string.IsNullOrWhiteSpace(telephone) ? reader["telephone"].ToString()! : telephone;
             }
         }
     
         string query2 = @"UPDATE EMPLOYE 
                           SET nom_employe = @nom,
                               prenom_employe = @prenom,
                               sexe = @sexe,
                               datenais = @datenais,
                               email = @email,
                               telephone = @telephone 
                           WHERE id_employe = @id";
     
         using var command2 = new MySqlCommand(query2, connection);
         command2.Parameters.AddWithValue("@id", id);
         command2.Parameters.AddWithValue("@nom", nom);
         command2.Parameters.AddWithValue("@prenom", prenom);
         command2.Parameters.AddWithValue("@sexe", sexe);
         command2.Parameters.Add("@datenais", MySqlDbType.Date).Value = dateNaissance;
         command2.Parameters.AddWithValue("@email", email);
         command2.Parameters.AddWithValue("@telephone", telephone);
     
         try
         {
             command2.ExecuteNonQuery();
             Console.WriteLine("modif reussi");
         }
         catch (MySqlException ex)
         {
             Console.WriteLine("Erreur MySQL : " + ex.Message);
         }
         catch (Exception e)
         {
             Console.WriteLine("Erreur générale : " + e.Message);
         }
     }

     public void DeleteEmployee(string? id)
     {
         using var connection = new MySqlConnection(_dbConnectionString);
         connection.Open();
         string query = @"delete from EMPLOYE where id_employe = @id";
         using var command = new MySqlCommand(query, connection);
         command.Parameters.AddWithValue("@id", id);
         try
         {
             command.ExecuteNonQuery();
         }
         catch (MySqlException ex)
         {
             Console.WriteLine("Erreur MySQL : " + ex.Message);
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
     }
}