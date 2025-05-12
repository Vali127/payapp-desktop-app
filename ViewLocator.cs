using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PayApp.ViewModels;

namespace PayApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param) // param prend la valeur du ViewModel de la currentView du MainViewModel.cs
    {
        if (param is null)
            return null;
        
        var viewName = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.InvariantCulture);  //viewName retourne le chemin vers le view séparé par des '.'
        var type = Type.GetType(viewName); //type du view

        if (type is null)
            return null;
        var control = (Control)Activator.CreateInstance(type)!;  //ca retourne une nouvelle instance du view
        control.DataContext = param;  // attaché les donnéés du view avec le viewModel
        
        return  control; //retourne une instantiation du view
    }

    public bool Match(object? data) => data is ViewModelBase;
}