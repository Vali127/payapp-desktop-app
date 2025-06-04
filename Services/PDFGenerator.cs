using System;
using System.IO;
using PayApp.DataModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace PayApp.Services;

public class PdfGenerator
{
    public static void GeneratePdf(string id)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        //les données
        PdfGeneratorData data;
        data = ServicesDataModel.GetPdfGeneratorData(id);
        // Récupère le chemin du dossier Téléchargements de l'utilisateur
        string downloadPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads",
            $"Reçu_de_{data.NameEmp}.pdf"
        );
        // Création du document PDF
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6.Landscape());
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content().Column(col =>
                {
                    col.Item().PaddingBottom(10).AlignCenter().Text("FICHE DE PAIE").SemiBold().FontSize(14).Underline();

                    col.Item().Row(row =>
                    {
                        row.Spacing(30); // Augmente l'espace entre les deux colonnes

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(t =>
                            {
                                t.Span("Date le : ").SemiBold().Underline();
                                t.Span($"{DateTime.Now:dd/MM/yyyy}").FontSize(10);
                            });

                            c.Item().Text(t =>
                            {
                                t.Span("Nom : ").SemiBold().Underline();
                                t.Span(data.NameEmp ?? "").FontSize(10);
                            });

                            c.Item().Text(t =>
                            {
                                t.Span("Prenom : ").SemiBold().Underline();
                                t.Span(data.Firstname ?? "").FontSize(10);
                            });

                            c.Item().Text(t =>
                            {
                                t.Span("Departement : ").SemiBold().Underline();
                                t.Span(data.Department ?? "").FontSize(10);
                            });

                            c.Item().Text(t =>
                            {
                                t.Span("Poste : ").SemiBold().Underline();
                                t.Span(data.Post ?? "").FontSize(10);
                            });
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignRight().Text(t =>
                            {
                                t.Span("Somme : ").SemiBold().Underline();
                                t.Span($"{data.Salary} ar").FontSize(10);
                            });

                            c.Item().PaddingTop(20).AlignRight().Text("Signature").Underline();
                        });
                    });

                    col.Item().PaddingTop(25).AlignCenter().Text($"L'employé {id} a reçu son salaire du mois de juin").FontSize(8);
                });
            });
        }).GeneratePdf(downloadPath);

    }
}
