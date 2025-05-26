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

public partial class AddEmployeViewModel:ObservableObject
{
    
    private EmployeeDataModel _dataModel = new EmployeeDataModel();
    public AddEmployeViewModel()
    {
        LoadPosts();
    }
    
    //recuperer les id postes exixtants
    private void LoadPosts()
    {
        var posts = _dataModel.GetIdPost();
        Idposts = new ObservableCollection<string>(posts);
    }
    //pour la liste deroulante de sexe
    public Window? ThisWindow { get; set; }
    [ObservableProperty] private ObservableCollection<string> _sexes = new()
    {
        "Feminin",
        "Masculin"
    };

    //donnees recuperer de view
    [ObservableProperty] private ObservableCollection<string> _idposts = new();
    [ObservableProperty] private string? _idPoste;
    [ObservableProperty] private string? _nomEmploye;
    [ObservableProperty] private string? _prenomEmploye;
    [ObservableProperty] private DateTimeOffset? _dateNaissance;
    [ObservableProperty] private string? _sexe;
    [ObservableProperty] private string? _telephone;
    [ObservableProperty] private string? _email;

    //ajout de l'employe avec validation des donnees
    [RelayCommand]
    private async Task AddNewEmploye()
    {
        if (string.IsNullOrWhiteSpace(NomEmploye) || string.IsNullOrWhiteSpace(PrenomEmploye) ||
            string.IsNullOrWhiteSpace(IdPoste) || string.IsNullOrWhiteSpace(Sexe) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Telephone)||DateNaissance==null)
        {
            var emptyBox = MessageBoxManager.GetMessageBoxStandard("Information incomplete", "Veuillez entrer toutes les donnees sur l'employe");
            await emptyBox.ShowAsync();
        }
        else if (!IsValidEmail(Email))
        {
            var emailBox = MessageBoxManager.GetMessageBoxStandard("Email invalide", "Veuillez verifier et corriger l'email entre");
            await emailBox.ShowAsync();
        }

        else if (!IsValidAge(DateNaissance))
        {
            var ageBox = MessageBoxManager.GetMessageBoxStandard("Date de naissance invalide", "Veuillez verifier et corriger la date de naissance entree ");
            await ageBox.ShowAsync();
        }

        else
        {

            var answer = _dataModel.InsertEmployee(IdPoste,NomEmploye, PrenomEmploye, Sexe, DateNaissance!.Value.Date, Email, Telephone);
            var box = MessageBoxManager.GetMessageBoxStandard("Operation ajout ", "Resultat : "+answer);
            var reload =  await box.ShowAsync();
          
            if (reload == ButtonResult.Ok)
            {
                ThisWindow?.Close();
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
        var age = today.Year - dateNaissance.Value.Date.Year;
        if (dateNaissance.Value.Date > today.AddYears(-age)) age--;

        if (age < 18)
        {
            return false;
        }
        else
        { 
            return true;
        }
        
    }

}