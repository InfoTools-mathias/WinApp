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
            txtID.Text = "";
            txtPW.Password = "";
            lblWrong.Content = "";
        }
        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblWrong.Content = "";
        }
        private void txtPW_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblWrong.Content = "";
        }
        #endregion

        async void btnSession_Click(object sender, RoutedEventArgs e)
        {
            string response = await api.auth(new User("", "", "", txtID.Text, 3, txtPW.Password));
            if (response == "OK")
            {
                gestion a = new gestion();
                a.Show();
                clear();
                this.Close();
            }
            else if (response == "Unauthorized")
            {
                txtPW.Password = "";
                lblWrong.Content = "Identifiant ou mot de passe invalide.";
            }
            else if(response == "error")
            {
                lblWrong.Content = "Problème lors de la connexion avec la base de donnée.";
            }
        }
    }
}
