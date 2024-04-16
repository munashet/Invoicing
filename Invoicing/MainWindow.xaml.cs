using Db4objects.Db4o;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Invoicing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {

        public IEmbeddedObjectContainer Db { get; set; }
        public Company Company { get; set; }
        public Setup Setup { get; set; }
        public List<Customer> Customers { get; set; }
        public User User { get; set; }
        private Customer SelectedCustomer;
        private Order SelectedOrder;
        private Item SelectedItem;
        private Category SelectedCategory;
        private Product SelectedProduct;

        public MainWindow() {
            try {
                InitializeComponent();
                var config = Db4oEmbedded.NewConfiguration();
                config.Common.UpdateDepth = 100;
                config.Common.ActivationDepth = 100;
                if (!Directory.Exists(@"C:\Munashe Software\Invoicing"))
                {
                    Directory.CreateDirectory(@"C:\Munashe Software\Invoicing");
                }
                Db = Db4oEmbedded.OpenFile(config, @"C:\Munashe Software\Invoicing\Invoicing.db");
                Company company = Db.Query<Company>().FirstOrDefault();
                if (company == null) {
                    company = new Company();
                    Db.Store(company);
                     CompanyWindow companyWindow = new CompanyWindow(this, company);
                      companyWindow.ShowDialog();
                      if (companyWindow.IsOk) {
                          Db.Store(company);
                       } else {
                       MessageBox.Show("You need to setup your company information", "Company Information...", MessageBoxButton.OK, MessageBoxImage.Warning);
                      }
                }
                Company = company;

                IEnumerable<Setup> setupDB = Db.Query<Setup>();
                List<Setup> setups = new List<Setup>(setupDB);
                if (setups.Count == 0) {
                    Setup setup = new Setup();
                    Db.Store(setup);
                    Setup = setup;
                } else {
                    Setup = setups[0];
                }
                Customers = new List<Customer>(Db.Query<Customer>());
                this.lvCustomers.ItemsSource = Customers;
                this.lvCategories.ItemsSource = Db.Query<Category>();
                this.lvProducts.ItemsSource = Db.Query<Product>();
            } catch (Exception ex) {
                LogException(this, ex);
            }
        }

        public void LogException(Window owner, Exception ex) {
            if (owner != null) {
                MessageBox.Show(owner, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        private void lvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (SelectedCustomer != null) {
                    Db.Store(SelectedCustomer);
                }
                SelectedCustomer = lvCustomers.SelectedItem as Customer;
                if (SelectedCustomer != null) {
                    this.lblCustomer.Content = SelectedCustomer.Name;
                    this.lvOrders.ItemsSource = SelectedCustomer.Orders;
                }
            } catch (Exception ex) {
                LogException(this, ex);
            }
        }

        private void lvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SelectedOrder = lvOrders.SelectedItem as Order;
            if (SelectedOrder != null) {
                this.lblOrders.Content = "Date of Order: " + SelectedOrder.OrderDateString + ";";
                this.lvItems.ItemsSource = SelectedOrder.Items;
            }
        }

        private void lvItems_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SelectedItem = lvItems.SelectedItem as Item;
            if (SelectedItem != null)
                this.lblItems.Content = SelectedItem.Description;
        }

        private void lvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SelectedCategory = lvCategories.SelectedItem as Category;
            if (SelectedCategory != null)
                this.lblCategories.Content = SelectedCategory.CategoryName;
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SelectedProduct = lvProducts.SelectedItem as Product;
            if (SelectedProduct != null)
                this.lblProducts.Content = SelectedProduct.ProductName;
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e) {
            if (SelectedCustomer == null) {
                MessageBox.Show("Select Customer to Edit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 0;
            } else {
                var cw = new CustomerWindow(this, SelectedCustomer, false);
                cw.Owner = this;
                cw.ShowDialog();
                lvCustomers.ItemsSource = Customers;
                lvCustomers.Items.Refresh();
                this.tabControl.SelectedIndex = 0;
            }
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e) {
            if (SelectedCustomer != null) {
                MessageBoxResult result = MessageBox.Show("Delete Customer " + SelectedCustomer.Name + "? \nAll data will be deleted", "Delete Customer", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    Customers.Remove(SelectedCustomer);
                    Db.Delete(SelectedCustomer);
                    lvCustomers.Items.Refresh();
                    lvOrders.ItemsSource = null;
                    lvItems.ItemsSource = null;
                    SelectedCustomer = null;
                    SelectedOrder = null;
                    SelectedItem = null;
                    this.tabControl.SelectedIndex = 0;
                }
            } else {
                MessageBox.Show("Select Customer to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 0;
            }
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e) {
            var c = new Customer(Company);
            var cw = new CustomerWindow(this, c, true);
            cw.Owner = this;
            cw.ShowDialog();
            Customers = new List<Customer>(Db.Query<Customer>());
            lvCustomers.ItemsSource = Customers;
            this.tabControl.SelectedIndex = 0;
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e) {
            if (SelectedCustomer == null) {
                MessageBox.Show("Select Customer to add orders", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 0;
                return;
            }
            this.tabControl.SelectedIndex = 1;
            MessageBoxResult result = MessageBox.Show("Add new Order?", "Add Order?", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Cancel)
                return;
            Order order = new Order(SelectedCustomer, Company.VAT);
            SelectedCustomer.Orders.Add(order);
            this.lvOrders.ItemsSource = SelectedCustomer.Orders;
            lvOrders.Items.Refresh();
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e) {
            if (SelectedOrder != null) {
                MessageBoxResult result = MessageBox.Show("Delete Order " + SelectedOrder.OrderDateString + "?\nAll data will be deleted", "Delete Order", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    SelectedCustomer.Orders.Remove(SelectedOrder);
                    SelectedOrder = null;
                }
                this.lvOrders.ItemsSource = SelectedCustomer.Orders;
                lvOrders.Items.Refresh();
                lvCustomers.Items.Refresh();
                this.tabControl.SelectedIndex = 1;
            } else {
                MessageBox.Show("Select Order to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 1;
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e) {
            if (SelectedOrder == null) {
                MessageBox.Show("Select Order to add items", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 1;
                return;
            }
            Item item = new Item();
            ItemWindow iw = new ItemWindow(this, SelectedOrder);
            iw.Owner = this;
            iw.ShowDialog();
            this.tabControl.SelectedIndex = 2;
            this.lvItems.ItemsSource = SelectedOrder.Items;
            lvItems.Items.Refresh();
            this.lvOrders.Items.Refresh();
            this.lvProducts.ItemsSource = Db.Query<Product>();
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e) {
            if (SelectedItem != null) {
                MessageBoxResult result = MessageBox.Show("Delete Item " + SelectedItem.Description + "?", "Delete Item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) {
                    SelectedOrder.Items.Remove(SelectedItem);
                    this.lvItems.ItemsSource = SelectedOrder.Items;
                    lvItems.Items.Refresh();
                    this.tabControl.SelectedIndex = 2;
                }
            } else {
                MessageBox.Show("Select Item to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 2;
            }
        }

        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (SelectedCustomer != null) {
                Db.Store(SelectedCustomer);
            }
            Db.Commit();
            Db.Close();
            Application.Current.Shutdown();
        }

        private void btnNewCategory_Click(object sender, RoutedEventArgs e) {
            Category category = new Category();
            CategoryWindow cw = new CategoryWindow(this, category, true);
            cw.Owner = this;
            cw.ShowDialog();
            lvCategories.ItemsSource = Db.Query<Category>();
        }

        private void btnEditCategory_Click(object sender, RoutedEventArgs e) {
            if (SelectedCategory != null) {
                var window = new CategoryWindow(this, SelectedCategory, false);
                window.Owner = this;
                window.ShowDialog();
                lvCategories.ItemsSource = Db.Query<Category>();
            } else
                MessageBox.Show("Select Category to Edit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            this.tabControl.SelectedIndex = 3;
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e) {
            if (SelectedCategory != null) {
                MessageBoxResult result = MessageBox.Show("Delete Category " + SelectedCategory.CategoryName + "?", "Delete Category", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) {
                    Db.Delete(SelectedCategory);
                    lvCategories.ItemsSource = Db.Query<Category>();
                    this.tabControl.SelectedIndex = 3;
                }
            } else {
                MessageBox.Show("Select Category to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 3;
            }
        }

        private void btnNewProduct_Click(object sender, RoutedEventArgs e) {
            var product = new Product();
            var window = new ProductWindow(this, product, true);
            window.Owner = this;
            window.ShowDialog();
            lvProducts.ItemsSource = Db.Query<Product>();
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e) {
            if (SelectedProduct != null) {
                var window = new ProductWindow(this, SelectedProduct, false);
                window.Owner = this;
                window.ShowDialog();
                lvProducts.ItemsSource = Db.Query<Product>();
            } else
                MessageBox.Show("Select Product to Edit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            this.tabControl.SelectedIndex = 4;
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e) {
            if (SelectedProduct != null) {
                MessageBoxResult result = MessageBox.Show("Delete Product " + SelectedProduct.ProductName + "?", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) {
                    Db.Delete(SelectedProduct);
                    SelectedProduct = null;
                    lvProducts.ItemsSource = Db.Query<Product>();
                }
            } else {
                MessageBox.Show("Select Product to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 4;
            }
        }

        private void miExit_Click(object sender, RoutedEventArgs e) {
            Db.Commit();
            Application.Current.Shutdown();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e) {
            string msg = "This product is registered to:\n";
            msg += Company.ToString();
            MessageBox.Show(msg, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnInvoice_Click(object sender, RoutedEventArgs e) {
            if (SelectedCustomer == null) {
                MessageBox.Show("No customer selected", "Invoice", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 0;
                return;
            }

            if (SelectedOrder == null) {
                MessageBox.Show("No Order selected", "Invoice", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 1;
                return;
            }

            this.lvItems.ItemsSource = SelectedOrder.Items;
            lvItems.Items.Refresh();

            if (SelectedOrder.Invoice == null) {
                var invoiceWindow = new InvoiceWindow(this, SelectedOrder);
                invoiceWindow.Owner = this;
                invoiceWindow.ShowDialog();
                if (invoiceWindow.IsOk) {
                    var invoice = invoiceWindow.Invoice;
                    var documentWindow = new DocumentWindow(invoice);
                    documentWindow.Owner = this;
                    documentWindow.ShowDialog();
                    this.lvCustomers.Items.Refresh();
                    this.lvOrders.Items.Refresh();
                }
            } else {
                var documentWindow = new DocumentWindow(SelectedOrder.Invoice);
                documentWindow.Owner = this;
                documentWindow.ShowDialog();
            }
        }

        private void btnQuotation_Click(object sender, RoutedEventArgs e) {
            if (SelectedCustomer == null) {
                MessageBox.Show("No customer selected", "Quotation", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 0;
                return;
            }

            if (SelectedOrder == null) {
                MessageBox.Show("No Order selected", "Quotation", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 1;
                return;
            }

            this.lvItems.ItemsSource = SelectedOrder.Items;
            lvItems.Items.Refresh();

            if (SelectedOrder.Quotation == null) {
                var quotationWindow = new QuotationWindow(this, SelectedOrder);
                quotationWindow.Owner = this;
                quotationWindow.ShowDialog();
                if (quotationWindow.IsOk) {
                    var quotation = quotationWindow.Quotation;
                    var documentWindow = new DocumentWindow(quotation);
                    documentWindow.Owner = this;
                    documentWindow.ShowDialog();
                }
            } else {
                var documentWindow = new DocumentWindow(SelectedOrder.Quotation);
                documentWindow.Owner = this;
                documentWindow.ShowDialog();
            }
        }

        private void btnReceipt_Click(object sender, RoutedEventArgs e) {
            if (SelectedCustomer == null) {
                MessageBox.Show("No customer selected", "Receipt", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 0;
                return;
            }

            if (SelectedOrder == null) {
                MessageBox.Show("No Order selected", "Receipt", MessageBoxButton.OK, MessageBoxImage.Error);
                this.tabControl.SelectedIndex = 1;
                return;
            }

            if (SelectedOrder.Invoice == null)
            {
                MessageBox.Show("You must first issue an invoice before you issue a receipt.", "Receipt", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.lvItems.ItemsSource = SelectedOrder.Items;
            lvItems.Items.Refresh();

            if (SelectedOrder.Receipt == null) {
                var receiptWindow = new ReceiptWindow(this, SelectedOrder);
                receiptWindow.Owner = this;
                receiptWindow.ShowDialog();
                if (receiptWindow.IsOk) {
                    SelectedOrder.Invoice.Paid = true;
                    var receipt = receiptWindow.Receipt;
                    var documentWindow = new DocumentWindow(receipt);
                    documentWindow.Owner = this;
                    documentWindow.ShowDialog();
                    this.lvCustomers.Items.Refresh();
                    this.lvOrders.Items.Refresh();
                }
            } else {
                var documentWindow = new DocumentWindow(SelectedOrder.Receipt);
                documentWindow.Owner = this;
                documentWindow.ShowDialog();
            }
        }

        private void windowLoaded(object sender, RoutedEventArgs e) {
            Login();
        }

        public void Login() {
            User user = Db.Query<User>().FirstOrDefault();
            if (user == null) {
                user = new User();
                user.Username = "admin";
                user.Password = "admin";
                Db.Store(user);
            }
            bool isOk = true;
            do {
                LoginWindow window = new LoginWindow();
                window.Owner = this;
                window.ShowDialog();
                if (window.IsCancelled) {
                    Close();
                    return;
                }
                user = Db.Query<User>().Where(x => x.Username == window.Username).FirstOrDefault();
                if (user == null) {
                    MessageBox.Show("Incorrect username or pasword", "Login", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    continue;
                } else {
                    if (user.Password != window.Password) {
                        MessageBox.Show("Incorrect username or password", "Login", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        continue;
                    } else {
                        User = user;
                        return;
                    }
                }
            } while (isOk); 
        }

        private void Login(object sender, RoutedEventArgs e) {
            Login();
        }

        private void miCompanyInformation_Click(object sender, RoutedEventArgs e) {
            CompanyWindow companyWindow = new CompanyWindow(this, Company);
            companyWindow.Owner = this;
            companyWindow.ShowDialog();
            if (companyWindow.IsOk) {
                Db.Store(Company);
                Db.Store(Setup);
            }
        }

        private void miLicence_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "Licence files (*.munlic)|*.munlic";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dialog.ShowDialog() == true)
            {
                var config = Db4oEmbedded.NewConfiguration();
                config.Common.UpdateDepth = 100;
                config.Common.ActivationDepth = 100;
                IEmbeddedObjectContainer Db = Db4oEmbedded.OpenFile(config, dialog.FileName);
                ExpiryDate ed = Db.Query<ExpiryDate>().FirstOrDefault();
                Company.ExpiryDate = ed;
                this.Db.Store(Company);
                Db.Close();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void MiChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow window = new ChangePasswordWindow(this);
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
