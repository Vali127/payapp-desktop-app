using Avalonia.Controls;
using Avalonia.Input;
using PayApp.ViewModels;

namespace PayApp.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.Handled = false;
        (DataContext as MainViewModel )?.ToggleSideMenuCommand.Execute(null);
    }
}