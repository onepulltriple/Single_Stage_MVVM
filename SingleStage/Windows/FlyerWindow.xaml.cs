using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SingleStage.Windows
{
    public partial class FlyerWindow : Window
    {
        private readonly FlowDocument _document;

        public FlyerWindow(FlowDocument document)
        {
            InitializeComponent();

            _document = document
                ?? throw new ArgumentNullException(nameof(document));

            FlyerReader.Document = _document;
        }

        private void PrintButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();

            if (printDialog.ShowDialog() != true)
                return;

            IDocumentPaginatorSource paginatorSource = _document;

            printDialog.PrintDocument(
                paginatorSource.DocumentPaginator,
                "Single Stage Flyer");
        }

        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}
