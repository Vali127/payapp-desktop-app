using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PayApp.ViewModels;

namespace PayApp.Views;

public partial class PaymentPageView : UserControl
{
    public PaymentPageView()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("button clicked");
        var btn = sender as Button;
        var id = btn!.Tag as string;
        (DataContext as PaymentPageViewModel) ?.GeneratePdfCommand.Execute(id);
    }
}