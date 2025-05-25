using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using PayApp.Dialog.DialogViewModel;

namespace PayApp.Dialog.DialogView;

public partial class AddPost : Window
{
    public AddPost()
    {
        InitializeComponent();
    }
    public AddPost(string? departId)
    {
        InitializeComponent();
        var vm = new PostViewModel();
        DataContext = vm;
        vm.SetCurrentDepartId(departId);
    }

    private void InputElementClose_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        (DataContext as PostViewModel)?.AddNewPostCommand.Execute(null);
    }
}