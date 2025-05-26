using System.Collections.ObjectModel;
using PayApp.DataModels;
using PayApp.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using CommunityToolkit.Mvvm.Input;


namespace PayApp.ViewModels;

public partial class EmployeePageViewModel : ViewModelBase
{ 
    
    private EmployeeDataModel _dataModel = new();
    //action pour bouton details
    [ObservableProperty]
    private ObservableCollection<Employee> _employees=new();
    public EmployeePageViewModel()
    {
        Employees = _dataModel.GetEmployees();
    }
    public void LoadEmployees()
    {
        Employees.Clear();
        foreach (var emp in new EmployeeDataModel().GetEmployees())
        {
            Employees.Add(emp);
        }
    }

    [RelayCommand]
    public void ReloadEmployees()
    {
        LoadEmployees();
    }
    public async Task ConfirmDeletionEmploye(Dictionary<string,object> data)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Confirmation","Voulez vous supprimer "+data["name"]+"de la liste des employes?", ButtonEnum.YesNo);
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