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

namespace Invoicing
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private MainWindow mainWindow;

        public ChangePasswordWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            User user = mainWindow.User;
            if (txtOldPassword.Password != user.Password)
            {
                MessageBox.Show("Old password is incorrect", "Change Password", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            if (txtNewPassword.Password != txtRepeatPassword.Password)
            {
                MessageBox.Show("New passwords don't match", "Change Password", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            user.Password = txtNewPassword.Password;
            mainWindow.Db.Store(user);
            mainWindow.Db.Commit();
            MessageBox.Show("Password changed successfully", "Change Password", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Hide();
        }
    }
}
