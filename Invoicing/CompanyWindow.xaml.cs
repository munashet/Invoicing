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
    /// Interaction logic for CompanyWindow.xaml
    /// </summary>
    public partial class CompanyWindow : Window {

        public Company Company { get; set; }
        public bool IsOk { get; set; }
        private MainWindow mainWindow;

        public CompanyWindow(MainWindow mainWindow, Company company) {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.Company = company;
            this.txtCompanyName.Text = Company.CompanyName;
            this.txtPhone.Text = Company.Phone;
            this.txtCell.Text = Company.Cell;
            this.txtEmail.Text = Company.Email;
            this.txtPhysicalAddress.Text = Company.PhysicalAddress;
            this.txtPostalAddress.Text = Company.PostalAddress;
            this.txtVAT.Text = company.VAT.ToString();
        }


        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                if (string.IsNullOrEmpty(txtCompanyName.Text)) {
                    MessageBox.Show(this, "Company Name cannot be blank", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                double vat = 0;
                if (!double.TryParse(txtVAT.Text, out vat))
                {
                    MessageBox.Show(this, "Incorrect number format for VAT", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Company.CompanyName = txtCompanyName.Text;
                Company.Phone = txtPhone.Text;
                Company.Cell = txtCell.Text;
                Company.Email = txtEmail.Text;
                Company.PhysicalAddress = txtPhysicalAddress.Text;
                Company.PostalAddress = txtPostalAddress.Text;
                Company.VAT = vat;
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
