using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
namespace PayApp.Views;
using Avalonia.Layout;
using Avalonia.Controls.ApplicationLifetimes;
using PayApp.DataModels;
using PayApp.ViewModels;

public partial class EmployeePageView : UserControl
{

    public EmployeePageView()
    {
        InitializeComponent();
    }

    public async void RefreshDatagrid()
    {
            if(this.DataContext is EmployeePageViewModel vm)
            {
                vm.LoadEmployees();
            }
    }

    public async void ShowMessage(string message)
    {
        var popup = new Window
        {
            Title = "Information",
            Width = 300,
            Height = 150,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        var button = new Button
        {
            Content = "OK",
            HorizontalAlignment = HorizontalAlignment.Right
        };

        button.Click += (_, _) => popup.Close();

        popup.Content = new StackPanel
        {
            Margin = new Thickness(20),
            Children =
            {
                new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,0,0,10) },
                button
            }
        };

        await popup.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow!);
    }


    public async void AddEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
        // Création des champs
        var nomBox = new TextBox { Watermark = "Nom", Margin = new Thickness(0, 0, 0, 10) };
        var prenomBox = new TextBox { Watermark = "Prénom", Margin = new Thickness(0, 0, 0, 10) };
        var dateNaissanceBox = new DatePicker { Margin = new Thickness(0, 0, 0, 10) };
        var sexeBox = new TextBox { Watermark = "Sexe", Margin = new Thickness(0, 0, 0, 10) };
        var emailBox = new TextBox { Watermark = "Email", Margin = new Thickness(0, 0, 0, 10) };
        var telBox = new TextBox { Watermark = "Numero tel", Margin = new Thickness(0, 0, 0, 10) };




        var enregistrerButton = new Button
        {
            Content = "Enregistrer",
            HorizontalAlignment = HorizontalAlignment.Right
        };

        var popup = new Window
        {
            Title = "Ajouter un employé",
            Width = 400,
            Height = 400,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Thickness(20),
                Children =
                {
                    new TextBlock { Text = "Nom :" },
                    nomBox,
                    new TextBlock { Text = "Prénom :" },
                    prenomBox,
                    new TextBlock { Text = "Date de naissance :" },
                    dateNaissanceBox,
                    new TextBlock { Text = "Sexe:" },
                    sexeBox,
                    new TextBlock { Text = "Email :" },
                    emailBox,
                    new TextBlock { Text = "Numero :" },
                    telBox,
                    enregistrerButton
                }
            }
        };
    
        enregistrerButton.Click += async (_, _) =>
        {
            var nom = nomBox.Text?.Trim();
            var prenom = prenomBox.Text?.Trim();
            var dateNaissance = dateNaissanceBox.SelectedDate;
            var sexe = sexeBox.Text?.Trim();
            var email = emailBox.Text?.Trim();
            var tel = telBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nom)||string.IsNullOrWhiteSpace(email)||string.IsNullOrWhiteSpace(tel) ||string.IsNullOrWhiteSpace(sexe)|| dateNaissance == null)
            { 
                ShowMessage("Veuillez remplir tous les champs importants");
                return;
            }

            // Envoie des données à une méthode d'enregistrement
            var model = new EmployeeDataModel();
            model.InsertEmployee(nom, prenom,sexe, dateNaissance.Value.DateTime,email,tel);

            popup.Close();
            RefreshDatagrid();
        };

        // Affiche le popup
        await popup.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow);
    }

    public async void DeleteEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
        var idBox = new TextBox { Watermark = "ID Employe", Margin = new Thickness(0, 0, 0, 10) };
        var confirmerButton = new Button
        {
            Content = "Confirmer",
            HorizontalAlignment = HorizontalAlignment.Right,
        };
        var popup = new Window
        {
            Title = "Supprimer un employé",
            Width = 400,
            Height = 120,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Thickness(20),
                Children =
                {
                    new TextBlock { Text = "ID Employe :" },
                    idBox,
                    confirmerButton
                }

            }
        };
        confirmerButton.Click += async (_, _) =>
        {
            var id = idBox.Text?.Trim();
       
  
            var model = new EmployeeDataModel();
            model.DeleteEmployee(id);
  
            popup.Close();
            RefreshDatagrid();
        };
        await popup.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow);
    }
  public async void ModifyEmployeePopup_Click(object? sender, RoutedEventArgs e)
  {
      // Création des champs
      var idBox = new TextBox{Watermark = "ID Employe", Margin = new Thickness(0, 0, 0, 10) };
      var nomBox = new TextBox { Watermark = "Nom", Margin = new Thickness(0, 0, 0, 10) };
      var prenomBox = new TextBox { Watermark = "Prénom", Margin = new Thickness(0, 0, 0, 10) };
      var dateNaissanceBox = new DatePicker { Margin = new Thickness(0, 0, 0, 10) };
      var sexeBox = new TextBox { Watermark = "Sexe", Margin = new Thickness(0, 0, 0, 10) };
      var emailBox = new TextBox { Watermark = "Email", Margin = new Thickness(0, 0, 0, 10) };
      var telBox = new TextBox { Watermark = "Numero tel", Margin = new Thickness(0, 0, 0, 10) };
  
      var enregistrerButton = new Button
      {
          Content = "Enregistrer les mofications",
          HorizontalAlignment = HorizontalAlignment.Right
      };
  
      var popup = new Window
      {
          Title = "Modifier un employé",
          Width = 400,
          Height = 450,
          CanResize = false,
          WindowStartupLocation = WindowStartupLocation.CenterOwner,
          Content = new StackPanel
          {
              Margin = new Thickness(20),
              Children =
              {
                  new TextBlock{Text = "Id:"},
                  idBox,
                  new TextBlock { Text = "Nom :" },
                  nomBox,
                  new TextBlock { Text = "Prénom :" },
                  prenomBox,
                  new TextBlock { Text = "Date de naissance :" },
                  dateNaissanceBox,
                  new TextBlock { Text = "Sexe:" },
                  sexeBox,
                  new TextBlock { Text = "Email :" },
                  emailBox,
                  new TextBlock { Text = "Numero :" },
                  telBox,
                  enregistrerButton
              }
          }
      };
  
      enregistrerButton.Click += async (_, _) =>
      {
          var id = idBox.Text?.Trim();
          var nom = nomBox.Text?.Trim();
          var prenom = prenomBox.Text?.Trim();
          DateTime? dateNaissance = dateNaissanceBox.SelectedDate?.DateTime;
          var sexe = sexeBox.Text?.Trim();
          var email = emailBox.Text?.Trim();
          var tel = telBox.Text?.Trim();
  
          var model = new EmployeeDataModel();
          model.UpdateEmployee(id, nom, prenom, sexe, dateNaissance, email, tel);
  
          popup.Close();
          RefreshDatagrid();
      };

      // Affiche le popup
      await popup.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
          ?.MainWindow);
  }
    
    
}

