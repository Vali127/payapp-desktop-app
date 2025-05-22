using System.Collections.ObjectModel;
using PayApp.Data;
using PayApp.DataModels;

namespace PayApp.ViewModels;

public class EmployeePageViewModel : ViewModelBase
{ 
    
    private EmployeeDataModel _dataModel = new EmployeeDataModel();

    public ObservableCollection<Employee> Employees{get;set;}
    
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
}