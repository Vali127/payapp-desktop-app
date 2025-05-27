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
        
        string query ="SELECT E.id_employe, E.nom_employe, E.prenom_employe,  E.datenais,  E.sexe,   E.email, E.telephone, E.id_poste,P.nom_poste,D.nom_departement FROM EMPLOYE E LEFT JOIN POSTE P ON E.id_poste = P.id_poste LEFT JOIN DEPARTEMENT D ON P.id_departement = D.id_departement;";
        
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
                IdPoste = reader["id_poste"].ToString()!,
                NomPoste = reader["nom_poste"].ToString()!,
                NomDepartement = reader["nom_departement"].ToString()!,
            };
            employees.Add(employee);
        }

        return employees;
    }
    //ajouter employe
    public string InsertEmployee(string idPoste,string nom, string prenom, string sexe,DateTime dateNaissance, string email, string telephone)
    {
        using var connection = new MySqlConnection(_dbConnectionString);
        connection.Open();
        string query = @"INSERT INTO EMPLOYE(id_poste,nom_employe, prenom_employe, datenais, email, telephone, sexe) 
                     VALUES (@idposte,@nom, @prenom, @datenais, @email, @telephone, @sexe)";
        
        try
        {
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idposte", idPoste);
            command.Parameters.AddWithValue("@nom", nom);
            command.Parameters.AddWithValue("@prenom", prenom);
            command.Parameters.AddWithValue("@datenais", dateNaissance);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@sexe", sexe);
            command.ExecuteNonQuery();
            return "Ajout de nouveau employe  reussi";
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Erreur MySQL : " + e.Message);
            return "Une erreur s'est produit lors de l'ajout";
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur générale : " + e.Message);
            return "Une erreur s'est produite lors de l'ajout";
        }
    }
    //modifier employe
   public string UpdateEmployee(string? idEmp,string? idpst, string? nom, string? prenom, string? sexe, DateTime? dateNaissance, string? email, string? telephone)
     {
         using var connection = new MySqlConnection(_dbConnectionString);
         connection.Open();
     
         string query1 = @"SELECT id_poste,nom_employe, prenom_employe, sexe, datenais, email, telephone 
                           FROM EMPLOYE WHERE id_employe = @id";
         using var command1 = new MySqlCommand(query1, connection);
         command1.Parameters.AddWithValue("@id", idEmp);
     
         using (var reader = command1.ExecuteReader())
         {
             if (reader.Read())
             {
                 idpst = string.IsNullOrWhiteSpace(idpst) ? reader["id_poste"].ToString()! : idpst;
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
                           SET id_poste = @idposte,
                               nom_employe = @nom,
                               prenom_employe = @prenom,
                               sexe = @sexe,
                               datenais = @datenais,
                               email = @email,
                               telephone = @telephone 
                           WHERE id_employe = @idEmp";
     
         try
         {
              using var command2 = new MySqlCommand(query2, connection);
              
              command2.Parameters.AddWithValue("@idEmp", idEmp);
              command2.Parameters.AddWithValue("@idposte", idpst);
              command2.Parameters.AddWithValue("@nom", nom);
              command2.Parameters.AddWithValue("@prenom", prenom);
              command2.Parameters.AddWithValue("@sexe", sexe);
              command2.Parameters.Add("@datenais", MySqlDbType.Date).Value = dateNaissance;
              command2.Parameters.AddWithValue("@email", email);
              command2.Parameters.AddWithValue("@telephone", telephone);
                      
             command2.ExecuteNonQuery();
             return("Operation de modification reussie");
         }
         catch (MySqlException sqlError)
         {
             Console.WriteLine("Erreur MySQL : " + sqlError.Message);
             return("Une Erreur s'est produite lors de la modification");
         }
         catch (Exception e)
         {
             Console.WriteLine("Erreur générale : " + e.Message);
             return("Une Erreur s'est produite lors de la modification");
         }
     }

     public string DeleteEmployee(string? id)
     {
         using var connection = new MySqlConnection(_dbConnectionString);
         connection.Open();
         string query = @"delete from EMPLOYE where id_employe = @id";
         
         try
         {
             using var command = new MySqlCommand(query, connection);
             command.Parameters.AddWithValue("@id", id);
             command.ExecuteNonQuery();
             return "Suppression reussie";
         }
         catch (MySqlException ex)
         {
             Console.WriteLine("Erreur MySQL : " + ex.Message);
             return "Une erreur s'est produit lors de la suppression";
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             return "Une erreur s'est produit lors de la suppression";

         }
     }

     //id post pour la liste deroulante
     public ObservableCollection<string> GetIdPost()
     {
         var idposts = new ObservableCollection<string>();
        
         using var connection = new MySqlConnection(_dbConnectionString);
         connection.Open();

         string query = "SELECT id_poste FROM POSTE";
         using var command = new MySqlCommand(query, connection);
         using var reader = command.ExecuteReader();
         while (reader.Read())
         {
             var id = reader["id_poste"].ToString();
             if (id != null) idposts.Add(id);
         }

         return idposts;
     }

     public ObservableCollection<Employee> GetSearchedEmployees(string? keyWordSearched)
     {
         var employees = new ObservableCollection<Employee>();
         var connection = new MySqlConnection(_dbConnectionString);
         connection.Open();
         
         string query = "SELECT E.id_employe, E.nom_employe, E.prenom_employe, E.datenais, E.sexe, E.email, E.telephone, E.id_poste, P.nom_poste, D.nom_departement FROM EMPLOYE E LEFT JOIN POSTE P ON E.id_poste = P.id_poste LEFT JOIN DEPARTEMENT D ON P.id_departement = D.id_departement WHERE E.nom_employe LIKE @word;";
         using var command = new MySqlCommand(query, connection);
         command.Parameters.AddWithValue("@word", $"%{keyWordSearched}%");
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
                 IdPoste = reader["id_poste"].ToString()!,
                 NomPoste = reader["nom_poste"].ToString()!,
                 NomDepartement = reader["nom_departement"].ToString()!,
             };
             employees.Add(employee);
         }

         return employees;
     }
}