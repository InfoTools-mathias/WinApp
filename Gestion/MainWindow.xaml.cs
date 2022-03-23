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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Newtonsoft.Json;
using System.Net.Http;

namespace Gestion
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtID.Focus();

            txtID.Text = "m.g@gmail.com";
            txtPW.Password = "azerty";
        }
        Api api = new Api();
        #region fonction clear()
        public void clear()
        {
            lblWrong.Visibility = Visibility.Hidden;
            lblWrong2.Visibility = Visibility.Hidden;
            txtID.Text = "";
            txtPW.Password = "";
        }
        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblWrong.Visibility = Visibility.Hidden;
        }
        private void txtPW_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblWrong.Visibility = Visibility.Hidden;
        }
        #endregion

        async void btnSession_Click(object sender, RoutedEventArgs e)
        {
            auth tmpAuth = new auth(txtID.Text, txtPW.Password);
            string response = await api.auth(tmpAuth);
            if (response == "OK")
            {
                gestion a = new gestion();
                a.Show();
                clear();
                this.Close();
            }
            else if(response == "error")
            {
                lblWrong2.Visibility = Visibility.Visible;
            }
            else
            {
                txtPW.Password = "";
                lblWrong.Visibility = Visibility.Visible;
            }
        }
    }
}
