using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PayApp.ViewModels;

namespace PayApp.Views;

public partial class OrgPageView : UserControl
{
    public OrgPageView()
    {
        InitializeComponent();
        DataContext = new OrgPageViewModel();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var button = (Button)sender!;
        var idDept = button.Tag as string;
        
    }
}