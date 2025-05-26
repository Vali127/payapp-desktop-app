using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using PayApp.Dialog.DialogViewModel;
namespace PayApp.Dialog.DialogView;

public partial class ModifyEmploye : Window
{
    public ModifyEmploye()
    {
        InitializeComponent();
    }
    public ModifyEmploye(string idEmp)
    {
        InitializeComponent();
        var vm = new ModifyEmployeViewModel();
        DataContext = vm;
        (DataContext as ModifyEmployeViewModel)?.EmployeIdGetCommand.Execute(idEmp);
        vm.ThisWindow = this;
        
    }

    private void InputElementClose_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }
}