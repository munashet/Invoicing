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
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window {

        private Category category;
        private bool isNew;
        private MainWindow mainWindow;

        public CategoryWindow(MainWindow mainWindow, Category category, bool isNew) {
            this.mainWindow = mainWindow;
            this.category = category;
            this.isNew = isNew;
            InitializeComponent();
            this.txtCategoryName.DataContext = category;
            this.txtDescription.DataContext = category;
            if (isNew)
                this.Title = "New Category";
            else
                this.Title = "Edit Category";
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) {
            try {
                Category category = mainWindow.Db.Query<Category>().Where(x => x.CategoryName == this.txtCategoryName.Text).FirstOrDefault();
                if (isNew && category != null) {
                    MessageBox.Show(this, "Category " + this.txtCategoryName.Text + " already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (isNew) {
                    category = new Category();
                    category.CategoryName = this.txtCategoryName.Text;
                }
                category.Description = this.txtDescription.Text;
                mainWindow.Db.Store(category);
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
