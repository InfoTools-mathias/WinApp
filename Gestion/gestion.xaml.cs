﻿using System;
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
        List<User> users = new List<User>();

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
            await client.Start();
            hide();

            // Product
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

            // User
            lstUser.Items.Clear();
            foreach (User u in client.users.cache)
            {
                users.Add(u);
                string data = "";
                data += u.name + " " + u.surname;
                data += "\nMail : " + u.mail;
                data += "\nType : " + client.users.GetStringType(u.type);
                lstUser.Items.Add(data);
            }

            // Meeting
            lstMeeting.Items.Clear();
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

            customersAtMeeting.Clear();
            customersNotAtMeeting.Clear();
            employeesAtMeeting.Clear();
            employeesNotAtMeeting.Clear();

            lstCustomerAtMeeting.Items.Clear();
            lstEmployeeAtMeeting.Items.Clear();
            lstCustomerNotAtMeeting.Items.Clear();
            lstEmployeeNotAtMeeting.Items.Clear();
            foreach (User u in client.users.cache)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                if (u.type == 0 || u.type == 1)
                {
                    employeesNotAtMeeting.Add(u);
                    lstEmployeeNotAtMeeting.Items.Add(data);
                }
                else
                {
                    customersNotAtMeeting.Add(u);
                    lstCustomerNotAtMeeting.Items.Add(data);
                }
            }

            // Product
            txtIDProduct.Text = "";
            txtNameProduct.Text = "";
            txtPriceProduct.Text = "";
            txtQuantityProduct.Text = "";
            txtDescriptionProduct.Text = "";
            cboUserFilter.Text = "Tous les type de compte";

            // Categorie
            lstCategorieInProduct.Items.Clear();
            lstCategorieForProduct.Items.Clear();
            categoriesForProduct.Clear();
            categoriesInProduct.Clear();
            foreach (Categorie c in client.categories.cache)
            {
                categoriesForProduct.Add(c);
                lstCategorieForProduct.Items.Add(c.name);
            }

            // User
            txtIDUser.Text = "";
            txtNameUser.Text = "";
            txtSurnameUser.Text = "";
            txtMailUser.Text = "";
            cboUserType.SelectedItem = null;
            txtPasswordUser.Password = "";
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

                lstCustomerNotAtMeeting.Items.Clear();
                lstEmployeeNotAtMeeting.Items.Clear();
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
                                lstEmployeeNotAtMeeting.Items.Add(data);
                            }
                            else
                            {
                                customersNotAtMeeting.Add(u);
                                lstCustomerNotAtMeeting.Items.Add(data);
                            }
                        }
                    }
                }

                lstCustomerAtMeeting.Items.Clear();
                lstEmployeeAtMeeting.Items.Clear();
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
                        lstEmployeeAtMeeting.Items.Add(data);
                    }
                    else
                    {
                        customersAtMeeting.Add(u);
                        lstCustomerAtMeeting.Items.Add(data);
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
        private void lstCustomerNotAtMeeting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCustomerNotAtMeeting.SelectedIndex >= 0)
            {
                customersAtMeeting.Add(customersNotAtMeeting[lstCustomerNotAtMeeting.SelectedIndex]);
                customersNotAtMeeting.RemoveAt(lstCustomerNotAtMeeting.SelectedIndex);
                refreshList();
            }
        }
        private void lstCustomerAtMeeting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCustomerAtMeeting.SelectedIndex >= 0)
            {
                customersNotAtMeeting.Add(customersAtMeeting[lstCustomerAtMeeting.SelectedIndex]);
                customersAtMeeting.RemoveAt(lstCustomerAtMeeting.SelectedIndex);
                refreshList();
            }
        }

        private void lstEmployeeNotAtMeeting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstEmployeeNotAtMeeting.SelectedIndex >= 0)
            {
                employeesAtMeeting.Add(employeesNotAtMeeting[lstEmployeeNotAtMeeting.SelectedIndex]);
                employeesNotAtMeeting.RemoveAt(lstEmployeeNotAtMeeting.SelectedIndex);
                refreshList();
            }
        }
        private void lstEmployeeAtMeeting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstEmployeeAtMeeting.SelectedIndex >= 0)
            {
                employeesNotAtMeeting.Add(employeesAtMeeting[lstEmployeeAtMeeting.SelectedIndex]);
                employeesAtMeeting.RemoveAt(lstEmployeeAtMeeting.SelectedIndex);
                refreshList();
            }
        }
        private void refreshList()
        {
            lstCustomerNotAtMeeting.Items.Clear();
            foreach (User u in customersNotAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstCustomerNotAtMeeting.Items.Add(data);
            }
            lstCustomerAtMeeting.Items.Clear();
            foreach (User u in customersAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstCustomerAtMeeting.Items.Add(data);
            }
            lstEmployeeNotAtMeeting.Items.Clear();
            foreach (User u in employeesNotAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstEmployeeNotAtMeeting.Items.Add(data);
            }
            lstEmployeeAtMeeting.Items.Clear();
            foreach (User u in employeesAtMeeting)
            {
                string data = "";
                data += u.id;
                data += "\n" + u.name + " " + u.surname;
                lstEmployeeAtMeeting.Items.Add(data);
            }
        }
        #endregion

        //product
        List<Categorie> categoriesForProduct = new List<Categorie>();
        List<Categorie> categoriesInProduct = new List<Categorie>();
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

                lstCategorieForProduct.Items.Clear();
                categoriesForProduct.Clear();
                foreach (Categorie c in client.categories.cache)
                {
                    foreach (Categorie c1 in productSelected.categories)
                    {
                        if (c.id != c1.id)
                        {
                            lstCategorieForProduct.Items.Add(c.name);
                            categoriesForProduct.Add(c);
                        }
                    }
                }

                lstCategorieInProduct.Items.Clear();
                categoriesForProduct.Clear();
                foreach (Categorie c in productSelected.categories)
                {
                    lstCategorieInProduct.Items.Add(c.name);
                    categoriesInProduct.Add(c);
                }
            }
            else
            {
                hide();
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
                    categoriesInProduct
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
                    categoriesInProduct
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
        #region select categorie
        private void lstCategorieForProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCategorieForProduct.SelectedIndex >= 0)
            {
                categoriesInProduct.Add(categoriesForProduct[lstCategorieForProduct.SelectedIndex]);
                categoriesForProduct.RemoveAt(lstCategorieForProduct.SelectedIndex);
                refreshListCategories();
            }
        }
        private void lstCategorieInProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCategorieInProduct.SelectedIndex >= 0)
            {
                categoriesForProduct.Add(categoriesInProduct[lstCategorieInProduct.SelectedIndex]);
                categoriesInProduct.RemoveAt(lstCategorieInProduct.SelectedIndex);
                refreshListCategories();
            }
        }
        private void refreshListCategories()
        {
            lstCategorieForProduct.Items.Clear();
            foreach (Categorie c in categoriesForProduct)
            {
                lstCategorieForProduct.Items.Add(c.name);
            }
            lstCategorieInProduct.Items.Clear();
            foreach (Categorie c in categoriesInProduct)
            {
                lstCategorieInProduct.Items.Add(c.name);
            }
        }
        #endregion

        //user
        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstUser.SelectedIndex >= 0)
            {
                User selectedUser = users[lstUser.SelectedIndex];

                txtIDUser.Text = selectedUser.id;
                txtNameUser.Text = selectedUser.name;
                txtSurnameUser.Text = selectedUser.surname;
                txtMailUser.Text = selectedUser.mail;
                cboUserType.Text = client.users.GetStringType(selectedUser.type);
                txtPasswordUser.Password = selectedUser.password;
            }
            else
            {
                hide();
            }
        }
        private async void userAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameUser.Text != "" || txtSurnameUser.Text != "" || txtMailUser.Text != "" || cboUserType.Text != null)
            {
                if (txtPasswordUser.Password == "" && (client.users.GetIntType(cboUserType.Text) == 0 || client.users.GetIntType(cboUserType.Text) == 1))
                {
                    lblWrong.Content = "Un compte emplyé ou administrateur nécessite un mot de passe.";
                }
                else
                {
                    User tmpUser = new User(
                        "",
                        txtNameUser.Text,
                        txtSurnameUser.Text,
                        txtMailUser.Text,
                        client.users.GetIntType(cboUserType.Text),
                        txtPasswordUser.Password
                    );
                    await client.users.Post(tmpUser);
                    refresh();
                }
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
                if (txtPasswordUser.Password == "" && (client.users.GetIntType(cboUserType.Text) == 0 || client.users.GetIntType(cboUserType.Text) == 1))
                {
                    lblWrong.Content = "Un compte emplyé ou administrateur nécessite un mot de passe.";
                }
                else
                {
                    User tmpUser = new User(
                        txtIDUser.Text,
                        txtNameUser.Text,
                        txtSurnameUser.Text,
                        txtMailUser.Text,
                        client.users.GetIntType(cboUserType.Text),
                        txtPasswordUser.Password
                    );
                    await client.users.Put(tmpUser);
                    refresh();
                }
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
        #region user settings
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
        private void cboUserFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (selected != null)
            {
                lstUser.SelectedItem = null;
                lstUser.Items.Clear();
                users.Clear();

                foreach (User u in client.users.cache)
                {
                    string data = "";
                    data += u.name + " " + u.surname;
                    data += "\nMail : " + u.mail;
                    data += "\nType : " + client.users.GetStringType(u.type);

                    if (selected == client.users.GetStringType(u.type))
                    {
                        lstUser.Items.Add(data);
                        users.Add(u);
                    }
                    else if (selected == "Tous les type de compte")
                    {
                        lstUser.Items.Add(data);
                        users.Add(u);
                    }
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
