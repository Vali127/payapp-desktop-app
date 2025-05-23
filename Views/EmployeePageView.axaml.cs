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
using System.Text.RegularExpressions;

public partial class EmployeePageView : UserControl
{

    public EmployeePageView()
    {
        InitializeComponent();
    }

    public async void RefreshDatagrid()
    {
        if (this.DataContext is EmployeePageViewModel vm)
        {
            vm.LoadEmployees();
        }
    }

    //pour les messages alertes ou confirmations des crud
    public void ShowMessage(string message, Window? owner = null)
    {
        var msg = new Window
        {
            Title = "Message",
            Width = 300,
            Height = 150,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var okButton = new Button
        {
            Content = "OK",
            HorizontalAlignment = HorizontalAlignment.Right
        };

        okButton.Click += (_, _) => msg.Close();

        msg.Content = new StackPanel
        {
            Margin = new Thickness(20),
            Children =
            {
                new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 10)
                },
                okButton
            }
        };

        if (owner != null)
        {
            msg.ShowDialog(owner);
        }
        else
        {
            msg.ShowDialog(new Window());
        }
    }



    public async void AddEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
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
                    new TextBlock { Text = "Numéro :" },
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

            // Champs requis
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(tel) ||
                string.IsNullOrWhiteSpace(sexe) || dateNaissance == null)
            {
                ShowMessage("Veuillez remplir tous les champs importants.");
                return;
            }

            // Vérification de l’âge (18 ans minimum)
            var today = DateTime.Today;
            var age = today.Year - dateNaissance.Value.Date.Year;
            if (dateNaissance.Value.Date > today.AddYears(-age)) age--;

            if (age < 18)
            {
                ShowMessage("L'employé doit avoir au moins 18 ans.", popup);
                return;
            }

            // Vérification de l’email
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                ShowMessage("L'adresse email est invalide. Veuillez la corriger.", popup);
                return;
            }



            // Insertion si tout est valide
            var model = new EmployeeDataModel();
            model.InsertEmployee(nom, prenom, sexe, dateNaissance.Value.DateTime, email, tel);

            popup.Close();
            RefreshDatagrid();

            ShowMessage("L’ajout de l’employé a été effectué avec succès !", popup);
        };

        await popup.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow);
    }

    //modifier un employe connaissant son id
    public async void ModifyEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
        var idBox = new TextBox { Watermark = "ID Employé", Margin = new Thickness(0, 0, 0, 10) };
        var nomBox = new TextBox { Watermark = "Nom", Margin = new Thickness(0, 0, 0, 10) };
        var prenomBox = new TextBox { Watermark = "Prénom", Margin = new Thickness(0, 0, 0, 10) };
        var dateNaissanceBox = new DatePicker { Margin = new Thickness(0, 0, 0, 10) };
        var sexeBox = new TextBox { Watermark = "Sexe", Margin = new Thickness(0, 0, 0, 10) };
        var emailBox = new TextBox { Watermark = "Email", Margin = new Thickness(0, 0, 0, 10) };
        var telBox = new TextBox { Watermark = "Numéro tel", Margin = new Thickness(0, 0, 0, 10) };

        var enregistrerButton = new Button
        {
            Content = "Enregistrer les modifications",
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
                    new TextBlock { Text = "Id:" },
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
                    new TextBlock { Text = "Numéro :" },
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
            var dateNaissance = dateNaissanceBox.SelectedDate;
            var sexe = sexeBox.Text?.Trim();
            var email = emailBox.Text?.Trim();
            var tel = telBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                ShowMessage("Veuillez entrer l'ID de l'employé à modifier.", popup);
                return;
            }

            // Vérifier l'âge si la date de naissance est modifiée
            if (dateNaissance != null)
            {
                var today = DateTime.Today;
                var age = today.Year - dateNaissance.Value.Year;
                if (dateNaissance > today.AddYears(-age)) age--;

                if (age < 18)
                {
                    ShowMessage("L'employé doit avoir au moins 18 ans.", popup);
                    return;
                }
            }

            // Vérifier l’email uniquement si l'utilisateur en a saisi un
            if (!string.IsNullOrWhiteSpace(email))
            {
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    ShowMessage("L'adresse email est invalide. Veuillez la corriger.", popup);
                    return;
                }
            }

            // Mise à jour
            var model = new EmployeeDataModel();
            model.UpdateEmployee(id, nom, prenom, sexe, dateNaissance.Value.DateTime, email, tel);

            popup.Close();
            RefreshDatagrid();
            ShowMessage("L’employé a été modifié avec succès.",
                (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow);
        };

        await popup.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow);
    }

    //SUPRIMER UN EMPLOYE
    public async void DeleteEmployeePopup_Click(object? sender, RoutedEventArgs e)
    {
        var idBox = new TextBox { Watermark = "ID Employé", Margin = new Thickness(0, 0, 0, 10) };
        var confirmerButton = new Button
        {
            Content = "Confirmer",
            HorizontalAlignment = HorizontalAlignment.Right,
        };

        var popup = new Window
        {
            Title = "Supprimer un employé",
            Width = 400,
            Height = 150,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Thickness(20),
                Children =
                {
                    new TextBlock { Text = "ID Employé :" },
                    idBox,
                    confirmerButton
                }
            }
        };

        confirmerButton.Click += async (_, _) =>
        {
            var id = idBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                ShowMessage("Veuillez entrer un ID valide.", popup);
                return;
            }

            // Créer une fenêtre de confirmation personnalisée
            var confirmationPopup = new Window
            {
                Title = "Confirmation",
                Width = 350,
                Height = 160,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var ouiButton = new Button
            {
                Content = "Oui",
                Width = 80,
                Margin = new Thickness(10)
            };

            var nonButton = new Button
            {
                Content = "Non",
                Width = 80,
                Margin = new Thickness(10)
            };

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Children = { ouiButton, nonButton }
            };

            confirmationPopup.Content = new StackPanel
            {
                Margin = new Thickness(20),
                Children =
                {
                    new TextBlock
                    {
                        Text = "Êtes-vous sûr de vouloir supprimer cet employé ?",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 20)
                    },
                    buttonPanel
                }
            };

            ouiButton.Click += (_, _) =>
            {
                var model = new EmployeeDataModel();
                model.DeleteEmployee(id);
                confirmationPopup.Close();
                popup.Close();
                RefreshDatagrid();
                ShowMessage("Employé supprimé avec succès !");
            };

            nonButton.Click += (_, _) =>
            {
                confirmationPopup.Close(); // juste fermer la confirmation
            };

            await confirmationPopup.ShowDialog(popup); // affiché au-dessus du popup principal
        };

        await popup.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow);
    }
}
