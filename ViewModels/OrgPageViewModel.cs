using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using PayApp.Data;
using PayApp.DataModels;

namespace PayApp.ViewModels;

public partial class OrgPageViewModel : ViewModelBase
{ 
    
    private readonly OrgDataModel _dataModel = new OrgDataModel();

    [ObservableProperty] private ObservableCollection<Department> _departments;
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

    [RelayCommand]
    private async Task PayFromPost(string id)
    {
        var response = OrgDataModel.PayAPost(id);
        var box = MessageBoxManager
            .GetMessageBoxStandard("Resultat", "Resultat de l' opération : "+response,
                ButtonEnum.Ok);

        await box.ShowAsync();
        
    }
}