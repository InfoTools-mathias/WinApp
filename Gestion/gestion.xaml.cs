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

using System.Net.Http;

namespace Gestion
{
    public partial class gestion : Window
    {
        public gestion()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            refresh();
        }
        Api api = new Api();
        List<meeting> cMeeting = new List<meeting>();
        List<product> cProduct = new List<product>();
        List<user> cUser = new List<user>();

        #region fonction refresh()
        async public void refresh()
        {
            try
            {
                hide();
                // PRODUCT : réinitialisation de la liste -> appel API -> remplissage de la liste
                lstProduct.Items.Clear();
                cProduct = await api.getProducts();
                foreach(product i in cProduct)
                {
                    string data = "";
                    data += i.name + "\n(";
                    bool j = true;
                    foreach(categorie y in i.categories)
                    {
                        if(j == false)
                        {
                            data +=  ", " + y.name;
                        }
                        else
                        {
                            data += y.name;
                            j = false;
                        }
                    }
                    data += ")\nQuantité : " + i.quantity;
                    data += "\nPrix : " + i.price;
                    lstProduct.Items.Add(data);
                }
                // USER : réinitialisation de la liste -> appel API -> remplissage de la liste
                lstUser.Items.Clear();
                cUser = await api.getUsers();
                foreach (user i in cUser)
                {
                    string data = "";
                    data += i.name + " " + i.surname;
                    data += "\nMail : " + i.mail;
                    data += "\nType : " + GetStringType(i.type);
                    lstUser.Items.Add(data);
                }
                // MEETING : réinitialisation des listes -> appel API -> remplissage des listes
                lstMeeting.Items.Clear();
                lstMeetingAllClient.Items.Clear();
                lstMeetingAllEmployee.Items.Clear();
                cMeeting = await api.getMeetings();
                foreach(meeting i in cMeeting)
                {
                    string data = "";
                    data += "Date : " + i.date;
                    data += "\nLieu : " + i.adress + " " + i.zip;
                    string dataEmployee = "";
                    string dataClient = "";
                    Boolean firstEmployee = true;
                    Boolean firstClient = true;
                    foreach (user user in i.users)
                    {
                        if(user.type == 0 || user.type == 1)
                        {
                            if(firstEmployee == true)
                            {
                                dataEmployee += user.name + " " + user.surname;
                                firstEmployee = false;
                            }
                            else
                            {
                                dataEmployee += "  |  " + user.name + " " + user.surname;
                            }
                        }
                        else
                        {
                            if (firstClient == true)
                            {
                                dataClient += user.name + " " + user.surname;
                                firstClient = false;
                            }
                            else
                            {
                                dataClient += "  |  " + user.name + " " + user.surname;
                            }
                        }
                    }
                    data += "\nEmployé(e)(s) : " + dataEmployee;
                    data += "\nClient(e)(s) : " + dataClient;
                    lstMeeting.Items.Add(data);
                }
                foreach(user user in cUser)
                {
                    string data = "";
                    data += user.id;
                    data += "\n" + user.name + " " + user.surname;
                    if(user.type == 0 || user.type == 1)
                    {
                        lstMeetingAllEmployee.Items.Add(data);
                    }
                    else
                    {
                        lstMeetingAllClient.Items.Add(data);
                    }
                }
            }
            catch
            {
                lblWrongMeeting.Visibility = Visibility.Visible;
                lblWrongProduct.Visibility = Visibility.Visible;
                lblWrongUser.Visibility = Visibility.Visible;
            }
        }
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            refresh();
        }
        #endregion

        #region fonction hide()
        public void hide()
        {
            lblWrongMeeting.Visibility = Visibility.Hidden;
            lblWrongProduct.Visibility = Visibility.Hidden;
            lblUnauthorizedProduct.Visibility = Visibility.Hidden;
            lblWrongUser.Visibility = Visibility.Hidden;
            lblWrongPassword.Visibility = Visibility.Hidden;
            lblWrongPassword2.Visibility = Visibility.Hidden;

            txtIDProduct.Text = "";
            txtNameProduct.Text = "";
            txtPriceProduct.Text = "";
            txtQuantityProduct.Text = "";
            txtDescriptionProduct.Text = "";
            image.Visibility = Visibility.Hidden;
            lstProduct.SelectedItem = null;

            txtIDUser.Text = "";
            txtNameUser.Text = "";
            txtSurnameUser.Text = "";
            txtMailUser.Text = "";
            txtPasswordUser.Password = "";
            //txtNewPassword.Password = "";
            //txtConfirmPassword.Password = "";
            cboUserType.SelectedItem = null;
            lstUser.SelectedItem = null;
        }
        private void hide_Click(object sender, RoutedEventArgs e)
        {
            hide();
        }
        #endregion

        //meeting
        private void lstMeeting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstMeeting.SelectedIndex > -1)
                {
                    meeting meetingSelected = cMeeting[lstMeeting.SelectedIndex];

                    txtIDMeeting.Text = meetingSelected.id;
                    txtDateMeeting.Text = Convert.ToString(meetingSelected.date);
                    txtZipMeeting.Text = meetingSelected.zip;
                    txtAdressMeeting.Text = meetingSelected.adress;

                    lstMeetingClient.Items.Clear();
                    lstMeetingEmployee.Items.Clear();
                    foreach (user user in meetingSelected.users)
                    {
                        string data = "";
                        data += user.id;
                        data += "\n" + user.name + " " + user.surname;
                        if (user.type == 0 || user.type == 1)
                        {
                            lstMeetingEmployee.Items.Add(data);
                        }
                        else
                        {
                            lstMeetingClient.Items.Add(data);
                        }
                    }

                    lstMeetingAllClient.Items.Clear();
                    lstMeetingAllEmployee.Items.Clear();
                    foreach (user user in cUser)
                    {
                        if (meetingSelected.users.Contains(user))
                        {
                            string data = "";
                            data += user.id;
                            data += "\n" + user.name + " " + user.surname;
                            if (user.type == 0 || user.type == 1)
                            {
                                lstMeetingAllEmployee.Items.Add(data);
                            }
                            else
                            {
                                lstMeetingAllClient.Items.Add(data);
                            }
                        }
                    }
                }
                else
                {
                    txtIDMeeting.Text = "";
                    txtDateMeeting.Text = "";
                    txtZipMeeting.Text = "";
                    txtAdressMeeting.Text = "";
                    foreach (user user in cUser)
                    {
                        string data = "";
                        data += user.id;
                        data += "\n" + user.name + " " + user.surname;
                        if (user.type == 0 || user.type == 1)
                        {
                            lstMeetingAllEmployee.Items.Add(data);
                        }
                        else
                        {
                            lstMeetingAllClient.Items.Add(data);
                        }
                    }
                    lstMeetingClient.Items.Clear();
                    lstMeetingEmployee.Items.Clear();
                }
            }
            catch
            {
                lblWrongMeeting.Visibility = Visibility.Visible;
            }
        }
        private async void meetingAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ()
                //{
                //    meeting tmpMeeting = new meeting();
                //    await api.putMeeting(tmpMeeting);
                //    refresh();
                //}
                //else
                //{
                //    lblWrongMeeting.Visibility = Visibility.Visible;
                //}
            }
            catch
            {
                lblWrongMeeting.Visibility = Visibility.Visible;
            }
        }
        private async void meetingModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ()
                //{
                //    meeting tmpMeeting = new meeting();
                //    await api.putMeeting(tmpMeeting);
                //    refresh();
                //}
                //else
                //{
                //    lblWrongMeeting.Visibility = Visibility.Visible;
                //}
            }
            catch
            {
                lblWrongMeeting.Visibility = Visibility.Visible;
            }
        }
        private async void meetingSupprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await api.deleteMeeting(txtIDMeeting.Text);
                refresh();
            }
            catch
            {
                lblWrongMeeting.Visibility = Visibility.Visible;
            }
        }

        //product
        private void lstProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstProduct.SelectedIndex > -1)
                {
                    product productSelected = cProduct[lstProduct.SelectedIndex];

                    txtIDProduct.Text = productSelected.id;
                    txtNameProduct.Text = productSelected.name;
                    txtPriceProduct.Text = Convert.ToString(productSelected.price);
                    txtQuantityProduct.Text = Convert.ToString(productSelected.quantity);
                    txtDescriptionProduct.Text = productSelected.description;
                }
                else
                {
                    txtIDProduct.Text = "";
                    txtNameProduct.Text = "";
                    txtPriceProduct.Text = "";
                    txtQuantityProduct.Text = "";
                    txtDescriptionProduct.Text = "";
                }
            }
            catch
            {
                lblWrongProduct.Visibility = Visibility.Visible;
            }
        }
        private async void productAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtNameProduct.Text != "" || txtPriceProduct.Text != "" || txtQuantityProduct.Text != "" || txtDescriptionProduct.Text != "")
                {
                    product tmpProduct = new product("", txtNameProduct.Text, Convert.ToDouble(txtPriceProduct.Text), Convert.ToInt32(txtQuantityProduct.Text), txtDescriptionProduct.Text, null);
                    await api.postProduct(tmpProduct);
                    refresh();
                }
                else
                {
                    lblWrongProduct.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                lblWrongProduct.Visibility = Visibility.Visible;
            }
        }
        private async void productModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtIDProduct.Text != "" || txtNameProduct.Text != "" || txtPriceProduct.Text != "" || txtQuantityProduct.Text != "" || txtDescriptionProduct.Text != "")
                {
                    product tmpProduct = new product(txtIDProduct.Text, txtNameProduct.Text, Convert.ToDouble(txtPriceProduct.Text), Convert.ToInt32(txtQuantityProduct.Text), txtDescriptionProduct.Text, null);
                    await api.putProduct(tmpProduct);
                    refresh();
                }
                else
                {
                    lblWrongProduct.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                lblWrongProduct.Visibility = Visibility.Visible;
            }
        }
        private async void productSupprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await api.deleteProduct(txtIDProduct.Text);
                refresh();
            }
            catch
            {
                lblWrongProduct.Visibility = Visibility.Visible;
            }
        }

        //user
        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstUser.SelectedIndex > -1)
                {
                    user selectedUser = cUser[lstUser.SelectedIndex];

                    txtIDUser.Text = selectedUser.id;
                    txtNameUser.Text = selectedUser.name;
                    txtSurnameUser.Text = selectedUser.surname;
                    txtMailUser.Text = selectedUser.mail;
                    cboUserType.Text = GetStringType(selectedUser.type);
                    txtPasswordUser.Password = selectedUser.password;
                }
                else
                {
                    txtIDUser.Text = "";
                    txtNameUser.Text = "";
                    txtSurnameUser.Text = "";
                    txtMailUser.Text = "";
                    cboUserType.SelectedValue = null;
                    txtPasswordUser.Password = "";
                }
            }
            catch
            {
                lblWrongUser.Visibility = Visibility.Visible;
            }
        }
        private async void userAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtNameUser.Text != "" || txtSurnameUser.Text != "" || txtMailUser.Text != "" || cboUserType.Text != null)
                {
                    user tmpUser = new user("", txtNameUser.Text, txtSurnameUser.Text, txtMailUser.Text, GetIntType(cboUserType.Text), txtPasswordUser.Password);
                    await api.postUser(tmpUser);
                    refresh();
                }
                else
                {
                    lblWrongUser.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                lblWrongUser.Visibility = Visibility.Visible;
            }
        }
        private async void userModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtIDUser.Text != "" || txtNameUser.Text != "" || txtSurnameUser.Text != "" || txtMailUser.Text != "" || cboUserType.SelectedValue != null)
                {
                    user tmpUser = new user(txtIDUser.Text, txtNameUser.Text, txtSurnameUser.Text, txtMailUser.Text, GetIntType(cboUserType.Text), txtPasswordUser.Password);
                    await api.putUser(tmpUser);
                    refresh();
                }
                else
                {
                    lblWrongUser.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                lblWrongUser.Visibility = Visibility.Visible;
            }
        }
        private async void userSupprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await api.deleteUser(txtIDUser.Text);
                refresh();
            }
            catch
            {
                lblWrongUser.Visibility = Visibility.Visible;
            }
        }
        private void cboUserType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems[0] as ComboBoxItem).Content as string == "administrateur" || (e.AddedItems[0] as ComboBoxItem).Content as string == "employé")
            {
                txtPasswordUser.IsEnabled = true;
            }
            else
            {
                txtPasswordUser.IsEnabled = false;
                txtPasswordUser.Password = "";
            }
        }
        //disconnect
        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                hide();
                api.killtoken();
                MainWindow a = new MainWindow();
                a.Show();
                this.Close();
            }
            catch
            {
                lblWrongProduct.Visibility = Visibility.Visible;
                lblWrongUser.Visibility = Visibility.Visible;
            }
        }

        private int GetIntType(string name)
        {
            switch (name)
            {
                case "administrateur":
                    return 0;
                case "employé":
                    return 1;
                case "client":
                    return 2;
                case "prospect":
                    return 3;
                default:
                    return 3;
            }
        }
        private string GetStringType(int id)
        {
            switch (id)
            {
                case 0:
                    return "administrateur";
                case 1:
                    return "employé";
                case 2:
                    return "client";
                case 3:
                    return "prospect";
                default:
                    return "prospect";
            }
        }
    }
}
