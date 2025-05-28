using System;
using Avalonia.Controls;
using Avalonia.Input;
using PayApp.ViewModels;

namespace PayApp.Views;

public partial class SettingPageView : UserControl
{
    public SettingPageView()
    {
        InitializeComponent();
        DataContext = new SettingPageViewModel();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is DateTimeOffset dto)
        {
            var dateOnly = dto.Date;
            var data = dateOnly.ToString("dd/MM/yyyy");
            (DataContext as SettingPageViewModel)?.UpdatePayCommand.Execute(data);
        }
    }
}