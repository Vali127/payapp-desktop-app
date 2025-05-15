using System.Collections.ObjectModel;
using PayApp.Data;
using PayApp.DataModels;

namespace PayApp.ViewModels;

public class OrgPageViewModel : ViewModelBase
{ 
    
    private OrgDataModel _dataModel = new OrgDataModel();

    public ObservableCollection<Department> Departments{get;set;}
    
    public OrgPageViewModel()
    {
        Departments = _dataModel.GetDepartments();
    }
}