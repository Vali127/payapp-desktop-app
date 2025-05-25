using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using PayApp.Dialog.DialogView;
using PayApp.ViewModels;
using ModifyPost = PayApp.Dialog.DialogView.ModifyPost;

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

    private void InputElementDeletePost_OnPointerPressed(object? sender, PointerPressedEventArgs e)
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

    private async void InputElementModifyPost_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var btn = (Button)sender!;
        var idPostClicked = btn.Tag as string;
        var name = btn.Name;
        
        var info = new Dictionary<string, object?>
        {
            ["id"] = idPostClicked,
            ["name"] = name
        };
        var formDialog = new ModifyPost(info);
        if (this.VisualRoot is Window mainWindow) await formDialog.ShowDialog(mainWindow);
        
    }

    private async void InputElementAddPopup_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var btn = (Button)sender!;
        var idDepart = btn.Tag as string;
        
        var formDialog = new AddPost(idDepart);
        if (this.VisualRoot is Window mainWindow) await formDialog.ShowDialog(mainWindow);
    }
}