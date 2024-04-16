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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        public bool IsOk { get; set; }
        public bool IsCancelled { get; set; }
        
        public LoginWindow() {
            InitializeComponent();
        }

        public string Username {
            get {
                return this.txtUsername.Text;
            }
        }

        public string Password {
            get {
                return this.txtPassword.Password;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            IsOk = true;
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            IsCancelled = true;
            this.Hide();
        }
    }
}
