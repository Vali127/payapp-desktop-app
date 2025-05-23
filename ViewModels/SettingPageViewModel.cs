using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using PayApp.DataModels;

namespace PayApp.ViewModels;

public partial class SettingPageViewModel : ViewModelBase
{
    private SettingPageDataModel _settingdataModel = new SettingPageDataModel();
    [ObservableProperty]  private DateTimeOffset? _selectedDate = SettingPageDataModel.GetSelectedDateFromDatabase();

    public SettingPageViewModel()
    {
        
    }

    [RelayCommand]
    private async void UpdatePay(string theDate)
    {
        var response = SettingPageDataModel.RestorePayment(theDate);
        
        var box = MessageBoxManager
            .GetMessageBoxStandard("Resultat", "Resultat de l' opération : "+response,
                ButtonEnum.Ok);

        await box.ShowAsync();
    }
}