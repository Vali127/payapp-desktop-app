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
        public int NumberOfPost { get; set; } = 0 ;
        public int NumberOfEmployees { get; set; } = 0 ;
}