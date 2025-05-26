using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace PayApp.Data;

public partial class Employee:ObservableObject
{
    public string IdEmploye {get;set;}=string.Empty;
    public string NomEmploye { get; set; } = string.Empty;
    public string PrenomEmploye { get; set; } = string.Empty;
    public string DateNaissance { get; set; } = string.Empty;
    public string NumTelephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Sexe { get; set; } = string.Empty;
    public string IdPoste { get; set; } = string.Empty;
    public string NomPoste { get; set; } = string.Empty;
    public string NomDepartement { get; set; } = string.Empty;
    [ObservableProperty]
    private bool _isDetailsVisible;
    [RelayCommand]
    private void ShowDetails() => IsDetailsVisible = !IsDetailsVisible;

    
}
