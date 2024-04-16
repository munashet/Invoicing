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
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window {

        private Product product;
        private bool isNew;
        private MainWindow mainWindow;

        public ProductWindow(MainWindow mainWindow, Product product, bool isNew) {
            this.mainWindow = mainWindow;
            this.product = product;
            this.isNew = isNew;
            InitializeComponent();
            this.cbCategory.ItemsSource = mainWindow.Db.Query<Category>();
            if (!isNew) {
                this.cbCategory.SelectedValue = product.Category.CategoryName;
            }
            this.DataContext = product;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) {
            double unitPrice;
            try {
                unitPrice = double.Parse(this.txtUnitPrice.Text, System.Globalization.CultureInfo.InvariantCulture);
            } catch (Exception ex) {
                this.lblStatus.Content = "Incorrect number format for UnitPrice";
                return;
            }
            int unitsInStock;
            try {
                unitsInStock = int.Parse(this.txtUnitsInStock.Text, System.Globalization.CultureInfo.InvariantCulture);
            } catch (Exception ex) {
                this.lblStatus.Content = "Incorrect number format for Units In Stock";
                return;
            }
            int unitsOnOrder;
            try {
                unitsOnOrder = int.Parse(this.txtUnitsOnOrder.Text, System.Globalization.CultureInfo.InvariantCulture);
            } catch (Exception ex) {
                this.lblStatus.Content = "Incorrect number format for Units On Order";
                return;
            }
            int reorderLevel;
            try {
                reorderLevel = int.Parse(this.txtReorderLevel.Text, System.Globalization.CultureInfo.InvariantCulture);
            } catch (Exception ex) {
                this.lblStatus.Content = "Incorrect number format for Reorder Level";
                return;
            }
            product.ProductName = string.IsNullOrEmpty(this.txtProductName.Text) ? " " : this.txtProductName.Text;
            product.Category = mainWindow.Db.Query<Category>().Where(x => x.CategoryName == this.cbCategory.SelectedValue.ToString()).FirstOrDefault();
            product.UnitPrice = unitPrice;
            product.UnitsInStock = unitsInStock;
            product.UnitsOnOrder = unitsOnOrder;
            product.ReorderLevel = reorderLevel;
            product.Discontinued = chkDiscontinued.IsChecked.Value;
            mainWindow.Db.Store(product);
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.Hide();
        }
    }
}
