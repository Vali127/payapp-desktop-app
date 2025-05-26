using System.Collections.ObjectModel;
using PayApp.Data;
using PayApp.DataModels;

namespace PayApp.ViewModels;

public class PaymentPageViewModel : ViewModelBase
{
    PaymentDataModel _paymentDataModel = new PaymentDataModel();
    public ObservableCollection<Payment> Payments {get;set;}

    public PaymentPageViewModel()
    {
        Payments = _paymentDataModel.GetPayedEmployee();
    }
}