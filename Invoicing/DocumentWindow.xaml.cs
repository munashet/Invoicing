using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Invoicing {
    /// <summary>
    /// Interaction logic for DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window {
        public DocumentWindow(Document doc) {
            InitializeComponent();
            this.Title = doc.GetTitle();
            this.flowDocumentReader.Document = doc.GetDocument();
            this.flowDocumentReader.ViewingMode = FlowDocumentReaderViewingMode.Scroll;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e) {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;

            this.flowDocumentReader.Document.PageHeight = pd.PrintableAreaHeight;
            this.flowDocumentReader.Document.PageWidth = pd.PrintableAreaWidth;
            this.flowDocumentReader.Document.ColumnWidth = pd.PrintableAreaWidth;

            IDocumentPaginatorSource idocument = this.flowDocumentReader.Document as IDocumentPaginatorSource;

            pd.PrintDocument(idocument.DocumentPaginator, "Printing " + this.Title + "...");
        }
    }
}
