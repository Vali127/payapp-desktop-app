using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using PayApp.Dialog.DialogViewModel;

namespace PayApp.Dialog.DialogView;

public partial class ModifyDepartment : Window
{
    public ModifyDepartment()
    {
        InitializeComponent();
    }
    public ModifyDepartment(Dictionary<string, object?> info)
    {
        InitializeComponent();
        var vm = new PostViewModel();
        DataContext = vm;
        ( DataContext as  PostViewModel )?.DepartInfoCommand.Execute(info);
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }
}