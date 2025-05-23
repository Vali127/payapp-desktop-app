using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
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
        var btn = (Button)sender!;
        var id = btn.Tag as string;
        ( DataContext as OrgPageViewModel )?.GetDepartementDetailsCommand.Execute(id);
    }

    private void InputElementPay_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var btn = (Button)sender!;
        var id = btn.Tag as string;
        (DataContext as OrgPageViewModel)?.PayFromPostCommand.Execute(id);
    }

    private void InputElementDelete_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var btn = (Button)sender!;
        var id = btn.Tag as string;
        var name = btn.Name;

        var info = new Dictionary<string, object?>
        {
            ["id"] = id,
            ["name"] = name
        };
        
        ( DataContext as OrgPageViewModel )?.ConfirmDeletionPostCommand.Execute(info!);
    }
}