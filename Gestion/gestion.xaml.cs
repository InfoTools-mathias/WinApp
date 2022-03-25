using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gestion
{
    public partial class gestion : Window
    {
        public gestion()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            refresh();
        }
        Api client = new Api();

        #region TextBox validation
        //Permet de ne n'accepter que les nombres
        private void DateValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\/[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\,[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void IntValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void txtHourMeeting_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtHourMeeting.Text != "")
            {
                if (Convert.ToDouble(txtHourMeeting.Text) > 24)
                {
                    txtHourMeeting.Text = "24";
                }
            }
        }
        private void txtMinuteMeeting_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMinuteMeeting.Text != "")
            {
                if (Convert.ToDouble(txtMinuteMeeting.Text) > 60)
                {
                    txtMinuteMeeting.Text = "60";
                }
            }
        }
        #endregion

        #region fonction refresh()
        async public void refresh()
        {
            hide();
            await client.Start();

            // PRODUCT : réinitialisation de la liste puis remplissage
            lstProduct.Items.Clear();
            foreach (Product p in client.products.cache)
            {
                string data = "";
                data += p.name + "\n(";
                bool first = true;
                foreach (Categorie c in p.categories)
                {
                    if (first == false)
                    {
                        data += ", " + c.name;
                    }
                    else
                    {
                        data += c.name;
                        first = false;
                    }
                }
                data += ")\nQuantité : " + p.quantity;
                data += "\nPrix : " + p.price;
                lstProduct.Items.Add(data);
            }

            // USER : réinitialisation de la liste puis remplissage
            lstUser.Items.Clear();
            foreach (User u in client.users.cache)
            {
                string data = "";
                data += u.name + " " + u.surname;
                data += "\nMail : " + u.mail;
                data += "\nType : " + client.users.GetStringType(u.type);
                lstUser.Items.Add(data);
            }

            // MEETING : réinitialisation des listes puis remplissage
            lstMeeting.Items.Clear();
            lstMeetingAllClient.Items.Clear();
            lstMeetingAllEmployee.Items.Clear();
            foreach (Meeting m in client.meetings.cache)
            {
                string data = "";
                data += "Date : " + m.date;
                data += "\nLieu : " + m.adress + " " + m.zip;

                string dataEmployee = "";
                string dataClient = "";
                Boolean firstEmployee = true;
                Boolean firstClient = true;
                foreach (User u in m.users)
                {
                    if (u.type == 0 || u.type == 1)
                    {
                        if (firstEmployee == true)
                        {
                            dataEmployee += u.name + " " + u.surname;
                            firstEmployee = false;
                        }
                        else
                        {
                            dataEmployee += "  |  " + u.name + " " + u.surname;
                        }
                    }
                    else
                    {
                        if (firstClient == true)
                        {
                            dataClient += u.name + " " + u.surname;
                            firstClient = false;
                        }
                        else
                        {
                            dataClient += "  |  " + u.name + " " + u.surname;
                        }
                    }
                }
                data += "\nEmployé(e)(s) : " + dataEmployee;
                data += "\nClient(e)(s) : " + dataClient;
                lstMeeting.Items.Add(data);
            }
            foreach (User u in client.users.cache)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                if (u.type == 0 || u.type == 1)
                {
                    employeesNotAtMeeting.Add(u);
                    lstMeetingAllEmployee.Items.Add(data);
                }
                else
                {
                    customersNotAtMeeting.Add(u);
                    lstMeetingAllClient.Items.Add(data);
                }
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
            // Message d'erreur
            lblWrong.Content = "";

            // Meeting
            txtIDMeeting.Text = "";
            txtDateMeeting.Text = "";
            txtHourMeeting.Text = "";
            txtMinuteMeeting.Text = "";
            txtZipMeeting.Text = "";
            txtAdressMeeting.Text = "";
            lstMeetingClient.Items.Clear();
            lstMeetingEmployee.Items.Clear();
            lstMeetingAllClient.Items.Clear();
            lstMeetingAllEmployee.Items.Clear();
            foreach (User u in client.users.cache)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                if (u.type == 0 || u.type == 1)
                {
                    lstMeetingAllEmployee.Items.Add(data);
                }
                else
                {
                    lstMeetingAllClient.Items.Add(data);
                }
            }
            customersAtMeeting.Clear();
            employeesAtMeeting.Clear();
            customersNotAtMeeting.Clear();
            employeesNotAtMeeting.Clear();

            // Product
            txtIDProduct.Text = "";
            txtNameProduct.Text = "";
            txtPriceProduct.Text = "";
            txtQuantityProduct.Text = "";
            txtDescriptionProduct.Text = "";
            lstProduct.SelectedItem = null;

            // User
            txtIDUser.Text = "";
            txtNameUser.Text = "";
            txtSurnameUser.Text = "";
            txtMailUser.Text = "";
            cboUserType.SelectedItem = null;
            txtPasswordUser.Password = "";
            lstUser.SelectedItem = null;
        }
        #endregion

        //meeting
        List<User> customersAtMeeting = new List<User>();
        List<User> employeesAtMeeting = new List<User>();
        List<User> customersNotAtMeeting = new List<User>();
        List<User> employeesNotAtMeeting = new List<User>();
        private void lstMeeting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMeeting.SelectedIndex >= 0)
            {
                Meeting meetingSelected = client.meetings.cache[lstMeeting.SelectedIndex];

                txtIDMeeting.Text = meetingSelected.id;
                txtDateMeeting.Text = Convert.ToString(meetingSelected.date);
                txtHourMeeting.Text = Convert.ToString(meetingSelected.date).Split(' ')[1].Split(':')[0];
                txtMinuteMeeting.Text = Convert.ToString(meetingSelected.date).Split(' ')[1].Split(':')[1];
                txtZipMeeting.Text = meetingSelected.zip;
                txtAdressMeeting.Text = meetingSelected.adress;

                lstMeetingAllClient.Items.Clear();
                lstMeetingAllEmployee.Items.Clear();
                customersNotAtMeeting.Clear();
                employeesNotAtMeeting.Clear();

                foreach (User u in client.users.cache)
                {
                    foreach (User u1 in meetingSelected.users)
                    {
                        if (u.id != u1.id)
                        {
                            string data = "";
                            data += u.id;
                            data += "\n" + u.name + " " + u.surname;
                            if (u.type == 0 || u.type == 1)
                            {
                                employeesNotAtMeeting.Add(u);
                                lstMeetingAllEmployee.Items.Add(data);
                            }
                            else
                            {
                                customersNotAtMeeting.Add(u);
                                lstMeetingAllClient.Items.Add(data);
                            }
                        }
                    }
                }

                lstMeetingClient.Items.Clear();
                lstMeetingEmployee.Items.Clear();
                customersAtMeeting.Clear();
                employeesAtMeeting.Clear();
                foreach (User u in meetingSelected.users)
                {
                    string data = "";
                    data += u.id;
                    data += "\n" + u.name + " " + u.surname;
                    if (u.type == 0 || u.type == 1)
                    {
                        employeesAtMeeting.Add(u);
                        lstMeetingEmployee.Items.Add(data);
                    }
                    else
                    {
                        customersAtMeeting.Add(u);
                        lstMeetingClient.Items.Add(data);
                    }
                }
            }
            else
            {
                hide();
            }
        }
        private async void meetingAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (txtDateMeeting.Text != "" || txtHourMeeting.Text != "" || txtMinuteMeeting.Text != "" || txtZipMeeting.Text != "" || txtAdressMeeting.Text != "" || customersAtMeeting.Count > 0)
            {
                List<User> users = new List<User>();
                foreach (User u in customersAtMeeting)
                {
                    users.Add(u);
                }
                foreach (User u in employeesAtMeeting)
                {
                    users.Add(u);
                }

                Meeting tmpMeeting = new Meeting(
                    "",
                    Convert.ToDateTime(txtDateMeeting.Text + " " + txtHourMeeting.Text + ":" + txtMinuteMeeting.Text + ":00"),
                    txtZipMeeting.Text,
                    txtAdressMeeting.Text,
                    users
                );
                await client.meetings.Post(tmpMeeting);
                refresh();
            }
            else
            {
                lblWrong.Content = "Tous les champs ne sont pas remplis";
            }
        }
        private async void meetingModifier_Click(object sender, RoutedEventArgs e)
        {
            if (txtIDMeeting.Text != "" || txtDateMeeting.Text != "" || txtHourMeeting.Text != "" || txtMinuteMeeting.Text != "" || txtZipMeeting.Text != "" || txtAdressMeeting.Text != "" || customersAtMeeting.Count > 0)
            {
                List<User> users = new List<User>();
                foreach (User u in customersAtMeeting)
                {
                    users.Add(u);
                }
                foreach (User u in employeesAtMeeting)
                {
                    users.Add(u);
                }

                Meeting tmpMeeting = new Meeting(
                    txtIDMeeting.Text,
                    Convert.ToDateTime(txtDateMeeting.Text + " " + txtHourMeeting.Text + ":" + txtMinuteMeeting.Text + ":00"),
                    txtZipMeeting.Text,
                    txtAdressMeeting.Text,
                    users
                );
                await client.meetings.Put(tmpMeeting);
                refresh();
            }
            else
            {
                lblWrong.Content = "Tous les champs ne sont pas remplis";
            }
        }
        private async void meetingSupprimer_Click(object sender, RoutedEventArgs e)
        {
            await client.meetings.Delete(txtIDMeeting.Text);
            refresh();
        }

        #region select user
        private void lstMeetingAllClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMeetingAllClient.SelectedIndex >= 0)
            {
                customersAtMeeting.Add(customersNotAtMeeting[lstMeetingAllClient.SelectedIndex]);
                customersNotAtMeeting.Remove(customersNotAtMeeting[lstMeetingAllClient.SelectedIndex]);
                refreshList();
            }
        }
        private void lstMeetingClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMeetingClient.SelectedIndex >= 0)
            {
                customersNotAtMeeting.Add(customersAtMeeting[lstMeetingClient.SelectedIndex]);
                customersAtMeeting.Remove(customersAtMeeting[lstMeetingClient.SelectedIndex]);
                refreshList();
            }
        }

        private void lstMeetingAllEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMeetingAllEmployee.SelectedIndex >= 0)
            {
                employeesAtMeeting.Add(employeesNotAtMeeting[lstMeetingAllEmployee.SelectedIndex]);
                employeesNotAtMeeting.Remove(employeesNotAtMeeting[lstMeetingAllEmployee.SelectedIndex]);
                refreshList();
            }
        }
        private void lstMeetingEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstMeetingEmployee.SelectedIndex >= 0)
            {
                employeesNotAtMeeting.Add(employeesAtMeeting[lstMeetingEmployee.SelectedIndex]);
                employeesAtMeeting.Remove(employeesAtMeeting[lstMeetingEmployee.SelectedIndex]);
                refreshList();
            }
        }
        private void refreshList()
        {
            lstMeetingAllClient.Items.Clear();
            foreach (User u in customersNotAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstMeetingAllClient.Items.Add(data);
            }
            lstMeetingClient.Items.Clear();
            foreach (User u in customersAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstMeetingClient.Items.Add(data);
            }
            lstMeetingAllEmployee.Items.Clear();
            foreach (User u in employeesNotAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstMeetingAllEmployee.Items.Add(data);
            }
            lstMeetingEmployee.Items.Clear();
            foreach (User u in employeesAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstMeetingEmployee.Items.Add(data);
            }
        }
        #endregion

        //product
        private void lstProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstProduct.SelectedIndex >= 0)
            {
                Product productSelected = client.products.cache[lstProduct.SelectedIndex];

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
        private async void productAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameProduct.Text != "" || txtPriceProduct.Text != "" || txtQuantityProduct.Text != "" || txtDescriptionProduct.Text != "")
            {
                Product tmpProduct = new Product(
                    "",
                    txtNameProduct.Text,
                    Convert.ToDouble(txtPriceProduct.Text),
                    Convert.ToInt32(txtQuantityProduct.Text),
                    txtDescriptionProduct.Text,
                    null
                );
                await client.products.Post(tmpProduct);
                refresh();
            }
            else
            {
                lblWrong.Content = "Tous les champs ne sont pas remplis";
            }
        }
        private async void productModifier_Click(object sender, RoutedEventArgs e)
        {
            if (txtIDProduct.Text != "" || txtNameProduct.Text != "" || txtPriceProduct.Text != "" || txtQuantityProduct.Text != "" || txtDescriptionProduct.Text != "")
            {
                Product tmpProduct = new Product(
                    txtIDProduct.Text,
                    txtNameProduct.Text,
                    Convert.ToDouble(txtPriceProduct.Text),
                    Convert.ToInt32(txtQuantityProduct.Text),
                    txtDescriptionProduct.Text,
                    null
                );
                await client.products.Put(tmpProduct);
                refresh();
            }
            else
            {
                lblWrong.Content = "Tous les champs ne sont pas remplis";
            }
        }
        private async void productSupprimer_Click(object sender, RoutedEventArgs e)
        {
            await client.products.Delete(txtIDProduct.Text);
            refresh();
        }

        //user
        string AccountToDisplay = "prospect";
        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstUser.SelectedIndex >= 0)
            {
                User selectedUser = client.users.cache[lstUser.SelectedIndex];

                txtIDUser.Text = selectedUser.id;
                txtNameUser.Text = selectedUser.name;
                txtSurnameUser.Text = selectedUser.surname;
                txtMailUser.Text = selectedUser.mail;
                cboUserType.Text = client.users.GetStringType(selectedUser.type);
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
        private async void userAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameUser.Text != "" || txtSurnameUser.Text != "" || txtMailUser.Text != "" || cboUserType.Text != null)
            {
                string password = txtPasswordUser.Password;
                if (txtPasswordUser.Password == "")
                {
                    password = client.users.cache[lstUser.SelectedIndex].password;
                }
                User tmpUser = new User(
                    "",
                    txtNameUser.Text,
                    txtSurnameUser.Text,
                    txtMailUser.Text,
                    client.users.GetIntType(cboUserType.Text),
                    password
                );
                await client.users.Post(tmpUser);
                refresh();
            }
            else
            {
                lblWrong.Content = "Tous les champs ne sont pas remplis";
            }
        }
        private async void userModifier_Click(object sender, RoutedEventArgs e)
        {
            if (txtIDUser.Text != "" || txtNameUser.Text != "" || txtSurnameUser.Text != "" || txtMailUser.Text != "" || cboUserType.SelectedValue != null)
            {
                string password = txtPasswordUser.Password;
                if (txtPasswordUser.Password == "")
                {
                    password = client.users.cache[lstUser.SelectedIndex].password;
                }
                User tmpUser = new User(
                    txtIDUser.Text,
                    txtNameUser.Text,
                    txtSurnameUser.Text,
                    txtMailUser.Text,
                    client.users.GetIntType(cboUserType.Text),
                    password
                );
                await client.users.Put(tmpUser);
                refresh();
            }
            else
            {
                lblWrong.Content = "Tous les champs ne sont pas remplis";
            }
        }
        private async void userSupprimer_Click(object sender, RoutedEventArgs e)
        {
            await client.users.Delete(txtIDUser.Text);
            refresh();
        }
        private void cboUserType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboUserType.SelectedValue != null)
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
        }
        #region read user
        private void cboUserFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboUserFilter.SelectedValue != null)
            {
                switch ((e.AddedItems[0] as ComboBoxItem).Content as string)
                {
                    case "Tous les type de compte":
                        AccountToDisplay = "all";
                        break;
                    case "Administrateur":
                        AccountToDisplay = "Administrateur";
                        break;
                    case "Employé":
                        AccountToDisplay = "Employé";
                        break;
                    case "Client":
                        AccountToDisplay = "Client";
                        break;
                    case "Prospect":
                        AccountToDisplay = "prospect";
                        break;
                    default:
                        AccountToDisplay = "prospect";
                        break;
                }
            }
        }
        #endregion

        //disconnect
        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            hide();
            client.killtoken();
            MainWindow a = new MainWindow();
            a.Show();
            this.Close();
        }
    }
}
