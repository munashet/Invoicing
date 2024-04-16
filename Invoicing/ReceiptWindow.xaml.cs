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
    /// Interaction logic for ReceiptWindow.xaml
    /// </summary>
    public partial class ReceiptWindow : Window {

        public Receipt Receipt { get; set; }
        public bool IsOk { get; set; }
        private MainWindow mainWindow;
        private Order order;

        public ReceiptWindow(MainWindow mainWindow, Order order) {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.order = order;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                Receipt receipt = new Receipt(order, mainWindow.Company.VAT);
                receipt.DocumentNumber = mainWindow.Setup.GetNextQuotationNumber();
                receipt.Notes = txtNotes.Text;
                receipt.Date = DateTime.Now;
                order.Receipt = receipt;
                receipt.Order = order;
                Receipt = receipt;
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
