using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PayApp.Dialog.DialogViewModel;

public partial class ModifyPostViewModel : ObservableObject
{
    [ObservableProperty] private string? _postId;
    [ObservableProperty] private string? _title;
    [RelayCommand]
    private void PostInfo( Dictionary<string, object> info )
    {
        PostId =  info["id"].ToString();
        Title = info["name"].ToString();
    }
}