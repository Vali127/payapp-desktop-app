using Avalonia;
using Avalonia.Controls;
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
}