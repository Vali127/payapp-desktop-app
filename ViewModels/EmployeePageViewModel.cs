using System;
using System.Collections.ObjectModel;
using PayApp.DataModels;
using PayApp.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using CommunityToolkit.Mvvm.Input;


namespace PayApp.ViewModels;

public partial class EmployeePageViewModel : ViewModelBase
{ 
    
    private EmployeeDataModel _dataModel = new();
    [ObservableProperty]  private string? _column = "nom";
    [ObservableProperty] private ObservableCollection<string> _columns = new()
    {
        "nom",
        "prenom",
        "sexe",
        "poste",
        "departement"
    };
    [ObservableProperty] private string? _pattern;
    //action pour bouton details
    [ObservableProperty]
    private ObservableCollection<Employee> _employees=new();
    
    public ICommand SearchCommand { get; }
    public ICommand AlldisplayCommand { get; }

    public EmployeePageViewModel()
    {
        Employees = _dataModel.GetEmployees();
        SearchCommand = new RelayCommand(LoadEmployeesSearch);
        AlldisplayCommand = new RelayCommand(LoadEmployees);
        PropertyChanged += (_, e) => Console.WriteLine($"Changement de propriété : {e.PropertyName}");

        
    }
    public void LoadEmployees()
    {
        Employees.Clear();
        foreach (var emp in new EmployeeDataModel().GetEmployees())
        {
            Employees.Add(emp);
        }
    }
    
public void LoadEmployeesSearch()
{
    Console.WriteLine("Recherche colonne: " + Column + ", pattern: " + Pattern);

    if (string.IsNullOrWhiteSpace(Column) || string.IsNullOrWhiteSpace(Pattern))
    {
        Console.WriteLine("Aucun critère de recherche fourni.");
        return;
    }

    Employees.Clear();
    foreach (var emp in _dataModel.GetEmployeeSearch(Column, Pattern))
    {
        Employees.Add(emp);
        Console.WriteLine("Résultat : " + emp.PrenomEmploye);
    }
}


    [RelayCommand]
    public void ReloadEmployees()
    {
        LoadEmployees();
    }
    public async Task ConfirmDeletionEmploye(Dictionary<string,object> data)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Confirmation","Voulez vous supprimer "+data["name"]+"  de la liste des employes?", ButtonEnum.YesNo);
        ButtonResult confirm = await box.ShowAsync();
        if (confirm  == ButtonResult.Yes)
        {
            var response = _dataModel.DeleteEmployee( data["id"] as string );
            var responseBox = MessageBoxManager.GetMessageBoxStandard("Resultat","Resultat : "+response);
            await responseBox.ShowAsync();
            LoadEmployees();
        }
    }
}