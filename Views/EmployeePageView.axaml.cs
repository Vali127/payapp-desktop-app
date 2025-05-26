using System.Collections.Generic;
using Avalonia.Controls;
using PayApp.ViewModels;
using Avalonia.Input;
using PayApp.Dialog.DialogView;
using AddEmploye = PayApp.Dialog.DialogView.AddEmploye;

namespace PayApp.Views;



public partial class EmployeePageView : UserControl
{

    public EmployeePageView()
    {
        InitializeComponent();
        DataContext=new EmployeePageViewModel();
    }

    private async void InputElementAddNewEmploye_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var dialogAdd = new AddEmploye();
        if (this.VisualRoot is Window mainWindow) await dialogAdd.ShowDialog(mainWindow);

    }
    private async void InputElementModifyEmploye_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        
        var btn = (Button)sender!;
        var id = btn.Tag as string;
        var dialogModify = new ModifyEmploye(id);
        if (this.VisualRoot is Window mainWindow) await dialogModify.ShowDialog(mainWindow);

        
    }
    private void InputElementDeleteEmploye_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var btn = (Button)sender!;
        var id = btn.Tag as string;
        var name = btn.Name;

        var info = new Dictionary<string, object?>
        {
            ["id"] = id,
            ["name"] = name
        };
        
        ( DataContext as EmployeePageViewModel )?.ConfirmDeletionEmploye(info!);     
    }


    private void InputRefresh_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var vm = new EmployeePageViewModel();
        vm.LoadEmployees();
    }
}
