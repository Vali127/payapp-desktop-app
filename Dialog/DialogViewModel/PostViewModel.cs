using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using PayApp.DataModels;

namespace PayApp.Dialog.DialogViewModel;

public partial class PostViewModel : ObservableObject
{
    OrgDataModel _dataModel = new OrgDataModel();
    public Window? ThisWindow { get; set; }
    [ObservableProperty] private string? _postId;
    [ObservableProperty] private string? _title;
    [RelayCommand]
    private void PostInfo( Dictionary<string, object> info )
    {
        PostId =  info["id"].ToString();
        Title = info["name"].ToString();
    }
    [ObservableProperty] private string? _currentDepartId;
    [ObservableProperty] private string? _currentDepartName;
    [RelayCommand]
    private void DepartInfo(Dictionary<string, object?> info)
    {
         CurrentDepartId = info["id"] as  string;
         CurrentDepartName = info["name"] as string;
    }

    public void SetCurrentDepartId(string? id)
    {
        CurrentDepartId = id;
    }
    
    [ObservableProperty] private string? _newPostName;
    [ObservableProperty] private string? _newPostDescription;

    [RelayCommand]
    private async Task GetNewPostInfo()
    {
        var response = _dataModel.UpdatePost(PostId, NewPostName, NewPostDescription);
        var box = MessageBoxManager.GetMessageBoxStandard("Resultat", "Resultat : "+response);
        var reload =  await box.ShowAsync();
        if (reload == ButtonResult.Ok)
        {
            ThisWindow?.Close();
        }
    }
    
    [ObservableProperty] private string? _postName; //pour l ajout
    [ObservableProperty] private string? _postDescription; //pour l' ajout
    [RelayCommand]
    private async Task AddNewPost()
    { 
        var response = _dataModel.AddNewPost(CurrentDepartId, PostName, PostDescription);
        var box = MessageBoxManager.GetMessageBoxStandard("Resultat", "Resultat : "+response);
        var closeWindow = await box.ShowAsync();
        if (closeWindow == ButtonResult.Ok)
        {
            ThisWindow?.Close();
        }
    }
    
    [ObservableProperty] private string? _newDepartName;
    [ObservableProperty] private string? _newDepartDescription;

    [RelayCommand]
    private async Task GetNewDepartInfo()
    {
        var response = _dataModel.UpdateDepartment(CurrentDepartId, NewDepartName, NewDepartDescription);
        var box = MessageBoxManager.GetMessageBoxStandard("Resultat", "Resultat : "+response);
        var reload =  await box.ShowAsync();
        if (reload == ButtonResult.Ok)
        {
            ThisWindow?.Close();
        }
    }
    
    [ObservableProperty] private string? _departName; //pour l ajout
    [ObservableProperty] private string? _departDescription; //pour l' ajout
    [RelayCommand]
    private async Task AddNewDepartment()
    { 
        var response = _dataModel.AddNewDepartment(DepartName, DepartDescription);
        var box = MessageBoxManager.GetMessageBoxStandard("Resultat", "Resultat : "+response);
        var closeWindow = await box.ShowAsync();
        if (closeWindow == ButtonResult.Ok)
        {
            ThisWindow?.Close();
        }
    }

}