using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Invoicing {
    /// <summary>
    /// Interaction logic for EditItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window {

        private Order order;
        private MainWindow mainWindow;

        public ItemWindow(MainWindow mainWindow, Order order) {
            this.mainWindow = mainWindow;
            this.order = order;
            InitializeComponent();
            IEnumerable<Category> categories = mainWindow.Db.Query<Category>();
            this.cbCategory.ItemsSource = categories;
            this.cbCategory.SelectedIndex = 0;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) {
            double unitPrice;
            try {
                unitPrice = double.Parse(this.txtUnitPrice.Text, System.Globalization.CultureInfo.InvariantCulture);
            } catch (Exception ex) {
                this.lblUnitPriceError.Content = "Incorrect number format for UnitPrice";
                return;
            }
            int quantity;
            try {
                quantity = int.Parse(this.txtQuantity.Text);
            } catch (Exception ex) {
                this.lblQuantityError.Content = "Incorrect number format for Quantity";
                return;
            }
            string description = "";
            var ob = this.cbProduct.SelectedValue;
            if (ob != null) {
                description = ob.ToString();
            } else {
                MessageBox.Show("Please select a product", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Item item = new Item();
            item.Description = description;
            item.Quantity = quantity;
            item.UnitPrice = unitPrice;
            order.Items.Add(item);
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.Hide();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                var ob = this.cbCategory.SelectedValue;
                if (ob != null) {
                    Category category = mainWindow.Db.Query<Category>().Where(x => x.CategoryName == ob.ToString()).FirstOrDefault();
                    if (category != null) {
                        IEnumerable<Product> list = mainWindow.Db.Query<Product>().Where(x => x.Category == category);
                        this.cbProduct.ItemsSource = list;
                        this.cbProduct.SelectedIndex = 0;
                    }
                }
            } catch (Exception ex) {
                mainWindow.LogException(this, ex);
            }
        }

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var ob = this.cbProduct.SelectedValue;
            if (ob != null) {
                Product product = mainWindow.Db.Query<Product>().Where(x => x.ProductName == ob.ToString()).FirstOrDefault();
                this.txtUnitPrice.DataContext = product;
            }
        }
    }
}
