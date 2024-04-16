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
using System.Data.SqlClient;

namespace Invoicing {
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window {

        public Invoice Invoice { get; set; }
        public bool IsOk { get; set; }
        private Order order;
        private MainWindow mainWindow;

        public InvoiceWindow(MainWindow mainWindow, Order order) {
            this.mainWindow = mainWindow;
            this.order = order;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.IsOk = false;
            this.Hide();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                Invoice invoice = new Invoice(order, mainWindow.Company.VAT);
                invoice.Date = DateTime.Now;
                invoice.Notes = string.IsNullOrEmpty(txtNotes.Text) ? "" : txtNotes.Text;
                invoice.DueDate = dpDueDate.SelectedDate.Value;
                invoice.DocumentNumber = mainWindow.Setup.GetNextInvoiceNumber();
                order.Invoice = invoice;
                invoice.Order = order;
                Invoice = invoice;
                this.IsOk = true;
                this.Hide();
            } catch (Exception ex) {
                mainWindow.LogException(this, ex);
            }
        }
    }
}
