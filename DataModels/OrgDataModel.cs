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
    
}