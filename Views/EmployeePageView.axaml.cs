using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PayApp.ViewModels;
using System.Text.RegularExpressions;
using Avalonia.Input;


namespace PayApp.Views;



public partial class EmployeePageView : UserControl
{

    public EmployeePageView()
    {
        InitializeComponent();
        DataContext=new EmployeePageViewModel();
    }
   
    public void RefreshDatagrid()
    {
        if (this.DataContext is EmployeePageViewModel vm)
        {
            vm.LoadEmployees();
        }
    }

    // Vérification de l’email
    public void IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            //que faire si email invalide
        }

    }

    // Vérification de l’âge (18 ans minimum)
    public void IsValidAge(DateTimeOffset? dateNaissance)
    {
        var today = DateTime.Today;
        if (dateNaissance != null)
        {
            var age = today.Year - dateNaissance.Value.Date.Year;
            if (dateNaissance.Value.Date > today.AddYears(-age)) age--;

            if (age < 18)
            {
                //action si age mineur
                
            }
        }
    }

    //ajouter les employes
    public void AddEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
        //contruction de boite de dialogue(idposte liste deroulante??,sexe aussi)
        // Boite de dialogue si champs non present
        //boite de dialogue si champs non valide email,datenaissance
        // Insertion si tout est valide
        // RefreshDatagrid();


    }

    //modifier un employe connaissant son id
    public void ModifyEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
        //id existant?? liste deroulante
        // Vérifier l'âge si la date de naissance est modifiée
        // Vérifier l’email uniquement si l'utilisateur en a saisi un
        // Mise à jour
    }

    //SUPRIMER UN EMPLOYE
    public void DeleteEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
        //boite de dialogue
        //verification si id existe ,liste deroulante?
        // Créer une fenêtre de confirmation personnalisée si oui effacer
        //refresh
    }
}
