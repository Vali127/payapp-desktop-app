using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using PayApp.DataModels;

namespace PayApp.Dialog.DialogViewModel;

public partial class ModifyPostViewModel : ObservableObject
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
}