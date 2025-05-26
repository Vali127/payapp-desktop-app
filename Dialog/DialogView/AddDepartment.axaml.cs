using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PayApp.Dialog.DialogViewModel;

namespace PayApp.Dialog.DialogView;

public partial class AddDepartment : Window
{
    public AddDepartment()
    {
        InitializeComponent();
        var vm = new PostViewModel();
        DataContext = vm;
        vm.ThisWindow = this;
    }

    private void InputElementClose_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }
    
}