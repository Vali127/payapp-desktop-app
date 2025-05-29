using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using PayApp.Data;
using PayApp.DataModels;
using PayApp.Services;

namespace PayApp.ViewModels;

public partial class PaymentPageViewModel : ViewModelBase
{
    PaymentDataModel _paymentDataModel = new PaymentDataModel();
    [ObservableProperty] private ObservableCollection<Payment> _payments;
    [ObservableProperty] private double _totalSalaireNet;
    [ObservableProperty] private long _totalEmployee;

    public PaymentPageViewModel()
    {
        Payments = _paymentDataModel.GetPayedEmployee();
        TotalSalaireNet = _paymentDataModel.GetSumOfPayedMoney();
        TotalEmployee = _paymentDataModel.GetSumOfPayedEmployee();
    }

    [RelayCommand]
    private void RefreshList()
    {
        Payments = _paymentDataModel.GetPayedEmployee();
        TotalSalaireNet = _paymentDataModel.GetSumOfPayedMoney();
        TotalEmployee = _paymentDataModel.GetSumOfPayedEmployee();
    }

    [RelayCommand]
    private async Task GeneratePdf(string id)
    {
        PdfGenerator.GeneratePdf(id);
        var box = MessageBoxManager.GetMessageBoxStandard("Resultat", "facturation effectuée !!");
        await box.ShowAsync();
    }
}