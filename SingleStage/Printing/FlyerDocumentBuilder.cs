using SingleStage.Models;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SingleStage.Printing
{
    public class FlyerDocumentBuilder
    {
        public FlowDocument Build(FlyerModel flyer)
        {
            var document = new FlowDocument
            {
                PageWidth = 793.7,   // A4 width at 96 DPI
                PageHeight = 1122.5, // A4 height at 96 DPI

                PagePadding = new Thickness(45),

                ColumnWidth = 350,
                ColumnGap = 28,

                FontFamily = new FontFamily("Ubuntu"),
                FontSize = 11
            };

            AddHeader(document, flyer);
            AddProgrammeHeading(document);
            AddPerformances(document, flyer);

            return document;
        }

        private static void AddHeader(
            FlowDocument document,
            FlyerModel flyer)
        {
            var title = new Paragraph
            {
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 8)
            };

            title.Inlines.Add(new Run(flyer.ShowName)
            {
                FontSize = 28,
                FontWeight = FontWeights.Bold
            });

            document.Blocks.Add(title);

            var date = new Paragraph
            {
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 2)
            };

            date.Inlines.Add(new Run(
                $"{flyer.StartTime:dddd, d MMMM yyyy}")
            {
                FontSize = 14,
                FontWeight = FontWeights.SemiBold
            });

            document.Blocks.Add(date);

            var time = new Paragraph
            {
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 4)
            };

            time.Inlines.Add(new Run(
                $"{flyer.StartTime:HH:mm} – {flyer.EndTime:HH:mm}")
            {
                FontSize = 14
            });

            document.Blocks.Add(time);

            if (flyer.TicketPrice.HasValue)
            {
                var ticketPrice = new Paragraph
                {
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 16)
                };

                ticketPrice.Inlines.Add(new Run(
                    $"Tickets: €{flyer.TicketPrice.Value:0.00}")
                {
                    FontSize = 14
                });

                document.Blocks.Add(ticketPrice);
            }
            else
            {
                // Maintain some spacing when there is no ticket price.
                document.Blocks.Add(
                    new Paragraph
                    {
                        Margin = new Thickness(0, 0, 0, 10)
                    });
            }

            AddDivider(document);
        }

        private static void AddProgrammeHeading(FlowDocument document)
        {
            var paragraph = new Paragraph
            {
                Margin = new Thickness(0, 12, 0, 12)
            };

            paragraph.Inlines.Add(new Run("PROGRAMME")
            {
                FontSize = 24,
                FontWeight = FontWeights.Bold
            });

            document.Blocks.Add(paragraph);
        }

        private static void AddPerformances(
            FlowDocument document,
            FlyerModel flyer)
        {
            foreach (FlyerPerformanceModel performance in
                     flyer.Performances.OrderBy(p => p.StartTime))
            {
                AddPerformance(document, performance);
            }
        }

        private static void AddPerformance(
            FlowDocument document,
            FlyerPerformanceModel performance)
        {
            // Performance time
            var timeParagraph = new Paragraph
            {
                Margin = new Thickness(0, 0, 0, 2)
            };

            timeParagraph.Inlines.Add(new Run(
                $"{performance.StartTime:HH:mm} – {performance.EndTime:HH:mm}")
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold
            });

            document.Blocks.Add(timeParagraph);


            // Performance name / description
            var descriptionParagraph = new Paragraph
            {
                Margin = new Thickness(0, 0, 0, 2)
            };

            descriptionParagraph.Inlines.Add(new Run(performance.Description)
            {
                FontSize = 19,
                FontWeight = FontWeights.SemiBold
            });

            document.Blocks.Add(descriptionParagraph);


            // Artists
            if (performance.Artists.Count > 0)
            {
                var artistsParagraph = new Paragraph
                {
                    Margin = new Thickness(20, 0, 0, 14)
                };

                artistsParagraph.Inlines.Add(new Run(
                    string.Join(" · ", performance.Artists))
                {
                    FontSize = 16,
                    Foreground = Brushes.DimGray
                });

                document.Blocks.Add(artistsParagraph);
            }
            else
            {
                // Preserve spacing when there are no artists.
                descriptionParagraph.Margin =
                    new Thickness(0, 0, 0, 14);
            }
        }

        private static void AddDivider(FlowDocument document)
        {
            var table = new Table
            {
                CellSpacing = 0,
                Margin = new Thickness(0, 0, 0, 4)
            };

            table.Columns.Add(new TableColumn());

            var rowGroup = new TableRowGroup();
            var row = new TableRow();

            var cell = new TableCell
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(0)
            };

            row.Cells.Add(cell);
            rowGroup.Rows.Add(row);
            table.RowGroups.Add(rowGroup);

            document.Blocks.Add(table);
        }
    }
}
