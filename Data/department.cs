namespace PayApp.Data;

public class Department
{
        public string IdDepartement { get; set; } = string.Empty ;
        public string NomDepartement { get; set; } = string.Empty ;
        public string Description { get; set; } = string.Empty ;
}

public class DepartmentDetails
{
        public string? DepartmentName { get; set; } = string.Empty ;
        public string? Description { get; set; } = string.Empty ;
        public long NumberOfPost { get; set; } = 0 ;
        public long NumberOfEmployees { get; set; } = 0 ;
}

public class PostOnEachDepartment
{
        public string? IdPoste { get; set; } = string.Empty;
        public string? NomPoste { get; set; } = string.Empty;
        public string? DescriptionPoste { get; set; } = string.Empty;
        public long NumberOfEmployees { get; set; } = 0;
}