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


    public ObservableCollection<DepartmentDetails> GetDepartementDetails( string idFromDepartment )
    {
        var departmentDetail = new ObservableCollection<DepartmentDetails>();
        
        using var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        var sql = "SELECT d.nom_departement, d.description_departement, COUNT(DISTINCT p.id_poste) AS nombre_postes, COUNT(e.id_employe) AS nombre_employes, IFNULL(SUM(s.salaire_base - s.impot), 0) - IFNULL(SUM(a.total_avance), 0) AS montant_total_a_payer FROM DEPARTEMENT d LEFT JOIN POSTE p ON d.id_departement = p.id_departement LEFT JOIN EMPLOYE e ON e.id_poste = p.id_poste LEFT JOIN SALAIRE s ON s.id_poste = p.id_poste LEFT JOIN (SELECT e.id_poste, e.id_employe, IFNULL(SUM(a.montant_avance), 0) AS total_avance FROM EMPLOYE e LEFT JOIN AVANCE a ON e.id_employe = a.id_employe GROUP BY e.id_poste, e.id_employe) a ON a.id_poste = p.id_poste AND a.id_employe = e.id_employe WHERE d.id_departement = '"+idFromDepartment+"' GROUP BY d.id_departement;\n";
        
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            var dept = new DepartmentDetails
            {
                DepartmentName = reader["nom_departement"] is DBNull ? "" : reader["nom_departement"].ToString(),
                Description = reader["description_departement"] is DBNull ? "" : reader["description_departement"].ToString(),
                NumberOfPost = reader["nombre_postes"] is DBNull ? 0 : Convert.ToInt64(reader["nombre_postes"]),
                NumberOfEmployees = reader["nombre_employes"] is DBNull ? 0 : Convert.ToInt64(reader["nombre_employes"]),
                Salary = Convert.ToDecimal(reader["montant_total_a_payer"])
            };


            departmentDetail.Add(dept);
        }
        
        return departmentDetail;
    }
    

    public ObservableCollection<PostOnEachDepartment> GetPostByDepartment(string idFromDepartment)
    {
        var result = new ObservableCollection<PostOnEachDepartment>();
        
        var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        var sql = "SELECT P.id_poste, P.nom_poste, P.description_poste, COUNT(E.id_employe) AS nb_employes, IFNULL(SUM(S.salaire_base), 0) AS salaire_brut_total, IFNULL(SUM(S.impot), 0) AS total_impot, IFNULL(MAX(A.total_avance), 0) AS total_avance, IFNULL(SUM(S.salaire_base - S.impot), 0) - IFNULL(MAX(A.total_avance), 0) AS salaire_net_total FROM POSTE P LEFT JOIN EMPLOYE E ON E.id_poste = P.id_poste LEFT JOIN SALAIRE S ON S.id_poste = P.id_poste LEFT JOIN (SELECT E.id_poste, SUM(A.montant_avance) AS total_avance FROM EMPLOYE E JOIN AVANCE A ON E.id_employe = A.id_employe GROUP BY E.id_poste) A ON P.id_poste = A.id_poste WHERE P.id_departement = '"+idFromDepartment+"' GROUP BY P.id_poste, P.nom_poste, P.description_poste;";
        
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var column = new PostOnEachDepartment()
            {
                IdPoste = reader["id_poste"].ToString()!,
                NomPoste = reader["nom_poste"].ToString()!,
                DescriptionPoste = reader["description_poste"].ToString()!,
                NumberOfEmployees = Convert.ToInt64(reader["nb_employes"]),
                Salary = Convert.ToDecimal(reader["salaire_brut_total"])
            };
            result.Add(column);
        }


        return result;
    }

    public static string PayAPost(string postId)
    {
        var dbconnectionsetting = Environment.GetEnvironmentVariable("DB_CONNECTION_SETTING"); 
        var connection = new MySqlConnection(dbconnectionsetting);
        connection.Open();

        var sql = "INSERT INTO PAIEMENT (id_employe, etat) SELECT E.id_employe, 'payé' FROM EMPLOYE E WHERE E.id_poste = '"+postId+"'";
        
        using var cmd = new MySqlCommand(sql, connection);
        try
        {
            cmd.ExecuteNonQuery();
            return "Paiement éffectué avec succes !!";
        }
        catch ( MySqlException error )
        {
            if ( error.Number == 1062 )  // j arrivais pas a identifier le genre d' exception donc je pars du code ' erreur pour poser des conditions
            {
                return "Paiement des employés non payé du poste !! \n à Notifier : pas de dédoublement pour les employés déja payé ce mois-ci ";
            }
            else
            {
                Console.WriteLine(error.Message, error.Number);
                return error.Message;
            }
        }
    }
    
    //_______________________code for post___________________________________

    public string DeletePost( string? idFromPost )
    {
        var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();
        var sql = "DELETE FROM POSTE WHERE id_poste = '"+idFromPost+"'";
        
        using var cmd = new MySqlCommand(sql, connection);
        try
        {
            cmd.ExecuteNonQuery();
            
            return "Poste supprimer avec sucees!" ;
        }
        catch (MySqlException error)
        {
            if (error.Number == 1451)
            {
                return "Suppression non permis !\nCe poste est toujours occupé";
            }
            return error.Number.ToString();
        }
    }

    public string UpdatePost(string? idFromPost, string? newName, string? newDescription)
    {
        var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();
        //getting old value
        var sql = " SELECT * FROM POSTE WHERE id_poste=@id ";
        var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", idFromPost);
        using var reader = cmd.ExecuteReader();

        //setting the new value
        if (reader.Read())
        {
            newName = string.IsNullOrEmpty(newName) ? reader.GetString("nom_poste") : newName;
            newDescription = string.IsNullOrEmpty(newDescription) ? reader.GetString("description_poste") : newDescription;
        }
        reader.Close();
        
        var sqlToUpdate = "UPDATE POSTE SET nom_poste = @name, description_poste = @desc WHERE id_poste = @id ;";
        try
        {

            using var cmd2 = new MySqlCommand(sqlToUpdate, connection);
            cmd2.Parameters.AddWithValue("@name", newName);
            cmd2.Parameters.AddWithValue("@desc", newDescription);
            cmd2.Parameters.AddWithValue("@id", idFromPost);
            cmd2.ExecuteNonQuery();

            return "Operation effectué avec succès !!";
        }
        catch (MySqlException error )
        {
            return error.Message;
        }
    }
    
    public string AddNewPost(string? idDepartement, string? name, string? description)
    {
        var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();

        var sql = " INSERT INTO POSTE(nom_poste,description_poste,id_departement) VALUES(@name,@description,@id) ";
        var cmd = new MySqlCommand(sql, connection);
        try
        {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@id", idDepartement);
            
            cmd.ExecuteNonQuery();
            return "Ajout de poste éfféctué !!";
        }
        catch (MySqlException e)
        {
            return e.Message;
        }
    }

    public string? GetDepartementOfPost(string? id)
    {
        var connection = new MySqlConnection(_dbconnectionsetting);
        connection.Open();
        var sql = "SELECT id_departement FROM POSTE WHERE id_poste=@id ";
        var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return reader.GetString("id_departement");
        }
        return null;
    }
}