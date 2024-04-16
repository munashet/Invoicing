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
    /// Interaction logic for QuotationWindow.xaml
    /// </summary>
    public partial class QuotationWindow : Window {

        public Quotation Quotation { get; set; }
        public bool IsOk { get; set; }
        private MainWindow mainWindow;
        private Order order;

        public QuotationWindow(MainWindow mainWindow, Order order) {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.order = order;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                int validFor;
                try {
                    validFor = int.Parse(this.txtValidFor.Text, System.Globalization.CultureInfo.InvariantCulture);
                } catch (Exception ex) {
                    MessageBox.Show(this, "Incorrect number format for \"valid for\"", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Quotation quote = new Quotation(order, mainWindow.Company.VAT);
                quote.DocumentNumber = mainWindow.Setup.GetNextQuotationNumber();
                quote.Notes = txtNotes.Text;
                quote.Date = DateTime.Now;
                quote.ValidFor = validFor;
                order.Quotation = quote;
                quote.Order = order;
                Quotation = quote;
                this.IsOk = true;
                this.Hide();
            } catch (Exception ex) {
                mainWindow.LogException(this, ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.IsOk = false;
            this.Hide();
        }
    }
}
