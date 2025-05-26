using Avalonia.Controls;
using PayApp.Dialog.DialogViewModel;
using Avalonia.Input;


namespace PayApp.Dialog.DialogView;

public partial class AddEmploye : Window
{
    public AddEmploye()
    {
        InitializeComponent();
        var vm = new AddEmployeViewModel();
        DataContext = vm;
        vm.ThisWindow = this;

    }

   
    
    private void InputElementClose_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }

}