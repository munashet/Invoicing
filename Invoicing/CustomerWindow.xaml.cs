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
using System.Data;
using System.Data.SqlClient;

namespace Invoicing {
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window {

        private Customer customer;
        private bool isNew;
        private MainWindow mainWindow;

        public CustomerWindow(MainWindow mainWindow, Customer customer, bool isNew) {
            this.mainWindow = mainWindow;
            this.customer = customer;
            this.isNew = isNew;
            InitializeComponent();
            this.cbType.ItemsSource = _GetCustomerTypes();
            if (!isNew) {
                this.txtName.Text = customer.Name;
                this.txtContactPerson.Text = customer.ContactPerson;
                this.txtEmail.Text = customer.Email;
                this.txtPhone.Text = customer.Phone;
                this.txtCell.Text = customer.Cell;
                this.txtPhysicalAddress.Text = customer.PhysicalAddress;
                this.txtPostalAddress.Text = customer.PostalAddress;
                this.cbType.SelectedValue = customer.CustomerType;
                this.lblCustomerDisplay.Content = customer.Name;
                this.Title = "Edit Customer";
            } else {
                this.lblCustomerDisplay.Content = "New Customer";
                this.Title = "New Customer";
            }
        }

        private List<string> _GetCustomerTypes() {
            var customerTypes = new List<string>();
            customerTypes.Add(Customer.COMPANY);
            customerTypes.Add(Customer.INDIVIDUAL);
            customerTypes.Add(Customer.NGO);
            customerTypes.Add(Customer.SCHOOL);
            customerTypes.Add(Customer.CHURCH);
            return customerTypes;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) {
            try {
                Customer customer = mainWindow.Db.Query<Customer>().Where(x => x.Name == txtName.Text).FirstOrDefault();
                if (isNew && customer != null) {
                    MessageBox.Show(this, "Customer " + txtName.Text + " already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (isNew) {
                    customer = new Customer(mainWindow.Company);
                    customer.Name = txtName.Text;
                }
                customer.CustomerType = cbType.SelectedValue + "";
                customer.ContactPerson = string.IsNullOrEmpty(txtContactPerson.Text) ? "" : txtContactPerson.Text;
                customer.PostalAddress = string.IsNullOrEmpty(txtPostalAddress.Text) ? "" : txtPostalAddress.Text;
                customer.PhysicalAddress = string.IsNullOrEmpty(txtPhysicalAddress.Text) ? "" : txtPhysicalAddress.Text;
                customer.Phone = string.IsNullOrEmpty(txtPhone.Text) ? "" : txtPhone.Text;
                customer.Cell = string.IsNullOrEmpty(txtCell.Text) ? "" : txtCell.Text;
                customer.Email = string.IsNullOrEmpty(txtEmail.Text) ? " " : txtEmail.Text;
                mainWindow.Db.Store(customer);
                this.Hide();
            } catch (Exception ex) {
                mainWindow.LogException(this, ex);
            }                
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.Hide();
        }
    }
}