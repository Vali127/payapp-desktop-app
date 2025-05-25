using System.Collections.Generic;
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
    public OrgPageViewModel()
    {
        Departments = _dataModel.GetDepartments();
    }
    
    [ObservableProperty] private bool _detailsIsShown ;
    private void DetailsShown()
    {
        DetailsIsShown = true;
    }
    
    
    [ObservableProperty]private string _currentDepartId = "";
    [ObservableProperty] private ObservableCollection<DepartmentDetails>? _departmentDetails;
    [ObservableProperty] private ObservableCollection<PostOnEachDepartment>? _postOnEachDepartments;
    [RelayCommand]
    private void GetDepartementDetails(string id)
    {
        DepartmentDetails = _dataModel.GetDepartementDetails(id);
        PostOnEachDepartments = _dataModel.GetPostByDepartment(id);
        CurrentDepartId = id;
        DetailsShown();
    }

    [RelayCommand]
    private async Task PayFromPost(string id)
    {
        var response = OrgDataModel.PayAPost(id);
        var box = MessageBoxManager.GetMessageBoxStandard("Resultat", "Resultat de l' opération : "+response);

        await box.ShowAsync();
        
    }

    [RelayCommand]
    private async Task ConfirmDeletionPost(Dictionary<string,object> data)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Confirmation","Voulez vous supprimer le poste"+data["name"]+"?", ButtonEnum.YesNo);
        ButtonResult confirm = await box.ShowAsync();
        if (confirm  == ButtonResult.Yes)
        {
            var response = _dataModel.DeletePost( data["id"] as string );
            var responseBox = MessageBoxManager.GetMessageBoxStandard("Resultat","Resultat : "+response);
            await responseBox.ShowAsync();
            GetDepartementDetails(CurrentDepartId);
        }
    }

    [RelayCommand]
    private async Task ConfirmDeletionDepartment(string id)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("confirmation", "Voulez vous supprimmer ce departement?", ButtonEnum.YesNo);
        ButtonResult confirmed = await box.ShowAsync();
        if (confirmed == ButtonResult.Yes)
        {
            var response = _dataModel.DeleteDepartement(id);
            var responseBox = MessageBoxManager.GetMessageBoxStandard("Resultat","Resultat : "+response);
            await responseBox.ShowAsync();
        }
    }

    [RelayCommand]
    private void RefreshPage()
    {
        Departments = _dataModel.GetDepartments();
        GetDepartementDetails(CurrentDepartId);
    }
}