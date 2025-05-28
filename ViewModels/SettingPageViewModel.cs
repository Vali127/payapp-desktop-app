using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using PayApp.DataModels;

namespace PayApp.ViewModels;

public partial class SettingPageViewModel : ViewModelBase
{
    [ObservableProperty]  private DateTimeOffset? _selectedDate = SettingPageDataModel.GetSelectedDateFromDatabase();

    [RelayCommand]
    private async Task UpdatePay(string theDate)
    {
        var response = SettingPageDataModel.RestorePayment(theDate);
        
        var box = MessageBoxManager
            .GetMessageBoxStandard("Resultat", "Resultat de l' opération : "+response);

        await box.ShowAsync();
    }
}