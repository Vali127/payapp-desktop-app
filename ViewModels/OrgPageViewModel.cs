using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PayApp.Data;
using PayApp.DataModels;

namespace PayApp.ViewModels;

public partial class OrgPageViewModel : ViewModelBase
{ 
    
    private readonly OrgDataModel _dataModel = new OrgDataModel();

    [ObservableProperty] public ObservableCollection<Department> _departments;
    [ObservableProperty] private ObservableCollection<DepartmentDetails>? _departmentDetails;
    [ObservableProperty] private ObservableCollection<PostOnEachDepartment>? _postOnEachDepartments;
    
    [ObservableProperty]
    private bool _detailsIsShown ;

    private void DetailsShown()
    {
        DetailsIsShown = true;
    } 
    
    public OrgPageViewModel()
    {
        Departments = _dataModel.GetDepartments();
    }

    [RelayCommand]
    private void GetDepartementDetails(string id)
    {
        DepartmentDetails = _dataModel.GetDepartementDetails(id);
        PostOnEachDepartments = _dataModel.GetPostByDepartment(id);
        DetailsShown();
    }
    
}