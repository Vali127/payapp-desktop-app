using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using PayApp.DataModels;
using System.Text.RegularExpressions;
using PayApp.ViewModels;

namespace PayApp.Dialog.DialogViewModel;

public partial class ModifyEmployeViewModel:ObservableObject
{
    
    private EmployeeDataModel _dataModel = new EmployeeDataModel();
    public ModifyEmployeViewModel()
    {
        LoadPosts();
    }
    private void LoadPosts()
    {
        var posts = _dataModel.GetIdPost();
        Idposts = new ObservableCollection<string>(posts);
    }
    
    public Window? ThisWindow { get; set; }
    
    //liste deroulante sexe
    [ObservableProperty] private ObservableCollection<string> _sexes = new()
    {
        "Feminin",
        "Masculin"
    };
    
    [ObservableProperty] private string? _idEmploye;
    [RelayCommand]
    private void EmployeIdGet(string idEmploye)=> IdEmploye=idEmploye;
    
    //nouveau donnees pour la modif
    [ObservableProperty] private ObservableCollection<string> _idposts = new();
    [ObservableProperty] private string? _idPoste;
    [ObservableProperty] private string? _nomEmploye;
    [ObservableProperty] private string? _prenomEmploye;
    [ObservableProperty] private DateTimeOffset? _dateNaissance;
    [ObservableProperty] private string? _sexe;
    [ObservableProperty] private string? _telephone;
    [ObservableProperty] private string? _email;
    
    [RelayCommand]
    private async Task ModifyEmploye()
    {
       
        
         if (Email != null && !IsValidEmail(Email))
        {
            var emailBox = MessageBoxManager.GetMessageBoxStandard("Email invalide", "Veuillez verifier et corriger l'email entre");
            await emailBox.ShowAsync();
        }

        else if (DateNaissance!=null&&!IsValidAge(DateNaissance))
        {
            var ageBox = MessageBoxManager.GetMessageBoxStandard("Date de naissance invalide", "Veuillez verifier et corriger la date de naissance entree ");
            await ageBox.ShowAsync();
        }

        else
        {

            var answer = _dataModel.UpdateEmployee(IdEmploye,IdPoste,NomEmploye, PrenomEmploye, Sexe, DateNaissance!.Value.Date, Email, Telephone);
            var box = MessageBoxManager.GetMessageBoxStandard("Operation ajout ", "Resultat : "+answer);
            var reload =  await box.ShowAsync();
          
            if (reload == ButtonResult.Ok)
            {
                ThisWindow?.Close();
                var vm = new EmployeePageViewModel();
                vm.LoadEmployees();
            }
        }
      
    }
 
    // Vérification de l’email et age si mineur
    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    private bool IsValidAge(DateTimeOffset? dateNaissance)
    {
        var today = DateTime.Today;
        if (dateNaissance != null)
        {
            var age = today.Year - dateNaissance.Value.Date.Year;
            if (dateNaissance.Value.Date > today.AddYears(-age)) age--;

            if (age < 18)
            {
                return false;
            }

            return true;
        }

        return false;
    }

}