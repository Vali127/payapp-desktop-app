using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using PayApp.Data;

namespace PayApp.DataModels;

public class OrgDataModel
{
    // ReSharper disable once StringLiteralTypo
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

        var sql = "SELECT d.nom_departement,  d.description_departement,  COUNT(DISTINCT p.id_poste) AS nombre_postes,  COUNT(e.id_employe) AS nombre_employes FROM  DEPARTEMENT d LEFT JOIN  POSTE p ON d.id_departement = p.id_departement LEFT JOIN  EMPLOYE e ON e.id_poste = p.id_poste WHERE  d.id_departement = '"+idFromDepartment+"'  GROUP BY  d.id_departement;";
        
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            var dept = new DepartmentDetails
            {
                DepartmentName = reader["nom_departement"] is DBNull ? "" : reader["nom_departement"].ToString(),
                Description = reader["description_departement"] is DBNull ? "" : reader["description_departement"].ToString(),
                NumberOfPost = reader["nombre_postes"] is DBNull ? 0 : Convert.ToInt64(reader["nombre_postes"]),
                NumberOfEmployees = reader["nombre_employes"] is DBNull ? 0 : Convert.ToInt64(reader["nombre_employes"])
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

        var sql = "SELECT P.id_poste,  P.nom_poste, P.description_poste, COUNT(E.id_employe) AS nb_employes FROM POSTE P LEFT JOIN EMPLOYE E ON E.id_poste = P.id_poste WHERE P.id_departement = '"+idFromDepartment+"' GROUP BY P.id_poste,P.nom_poste;";
        
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var column = new PostOnEachDepartment()
            {
                IdPoste = reader["id_poste"].ToString()!,
                NomPoste = reader["nom_poste"].ToString()!,
                DescriptionPoste = reader["description_poste"].ToString()!,
                NumberOfEmployees = Convert.ToInt64(reader["nb_employes"])
            };
            result.Add(column);
        }


        return result;
    }
    
}