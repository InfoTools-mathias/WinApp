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

/** Comment récupérer les données ?
 * 1. les fonctions globals pour faire des appels API sont dans "services/api.cs". Des instances de chaque services y sont définie.
 * 2. Dans chaque services (UserService, ProductService, etc..) se trouve les fonctions Get, Post, Put et Delete spécifique à chaque Objet.
 * 3. Dans ce fichier, pour appeler l'api il suffit de faire client.objects.Get/Post/Put/Delete.
 */

/** Explication des listes :
 * lstNomDeList -> listeBox de l'interface.
 * nomDeList -> List<Object> pour stocké les valeurs réelle des objets.
 * client.Object.cache -> retourne les listes complètes des objets, ces listes ne doivent jamais être modifié, sauf lors des appels api.
 */

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

        #region TextBox validation (Regex)
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

            // Categorie
            lstCategorie.Items.Clear();
            foreach (Categorie c in client.categories.cache)
            {
                lstCategorie.Items.Add(c.name);
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
                if (u.type == 0 || u.type == 1)
                {
                    employeesNotAtMeeting.Add(u);
                    lstEmployeeNotAtMeeting.Items.Add(u.name + " " + u.surname);
                }
                else
                {
                    customersNotAtMeeting.Add(u);
                    lstCustomerNotAtMeeting.Items.Add(u.name + " " + u.surname);
                }
            }

            // Product
            txtIDProduct.Text = "";
            txtNameProduct.Text = "";
            txtPriceProduct.Text = "";
            txtQuantityProduct.Text = "";
            txtDescriptionProduct.Text = "";
            cboUserFilter.Text = "Tous les types de compte";

            // Categorie
            txtNameCategorie.Text = "";

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
            cboUserType.Text = null;

            txtPasswordUser.Password = "";
            txtPasswordUser.Visibility = Visibility.Hidden;
            lblPassword.Visibility = Visibility.Hidden;
            lblFacture.Margin = new Thickness(1025, 25, 0, 0);
            lstFactureUser.Margin = new Thickness(1025, 50, 0, 0);
            lstFactureUser.Height = 275;

            btnCreateFacture.Visibility = Visibility.Hidden;
            lstFactureUser.Items.Clear();
            facture.Visibility = Visibility.Hidden;
            txtIDFacture.Text = "";
            txtDateFacture.Text = "";
            txtClientFacture.Text = "";
        }
        #endregion

        //meeting
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
                            if (u.type == 0 || u.type == 1)
                            {
                                employeesNotAtMeeting.Add(u);
                                lstEmployeeNotAtMeeting.Items.Add(u.name + " " + u.surname);
                            }
                            else
                            {
                                customersNotAtMeeting.Add(u);
                                lstCustomerNotAtMeeting.Items.Add(u.name + " " + u.surname);
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
                    if (u.type == 0 || u.type == 1)
                    {
                        employeesAtMeeting.Add(u);
                        lstEmployeeAtMeeting.Items.Add(u.name + " " + u.surname);
                    }
                    else
                    {
                        customersAtMeeting.Add(u);
                        lstCustomerAtMeeting.Items.Add(u.name + " " + u.surname);
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
        List<User> customersAtMeeting = new List<User>();
        List<User> employeesAtMeeting = new List<User>();
        List<User> customersNotAtMeeting = new List<User>();
        List<User> employeesNotAtMeeting = new List<User>();
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
                lstCustomerNotAtMeeting.Items.Add(u.name + " " + u.surname);
            }
            lstCustomerAtMeeting.Items.Clear();
            foreach (User u in customersAtMeeting)
            {
                lstCustomerAtMeeting.Items.Add(u.name + " " + u.surname);
            }
            lstEmployeeNotAtMeeting.Items.Clear();
            foreach (User u in employeesNotAtMeeting)
            {
                lstEmployeeNotAtMeeting.Items.Add(u.name + " " + u.surname);
            }
            lstEmployeeAtMeeting.Items.Clear();
            foreach (User u in employeesAtMeeting)
            {
                lstEmployeeAtMeeting.Items.Add(u.name + " " + u.surname);
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

                lstCategorieForProduct.Items.Clear();
                categoriesForProduct.Clear();
                foreach (Categorie c in client.categories.cache)
                {
                    lstCategorieForProduct.Items.Add(c.name);
                    categoriesForProduct.Add(c);
                    foreach (Categorie c1 in productSelected.categories)
                    {
                        if (c.id == c1.id)
                        {
                            lstCategorieForProduct.Items.Remove(c.name);
                            categoriesForProduct.Remove(c);
                        }
                    }
                }

                lstCategorieInProduct.Items.Clear();
                categoriesInProduct.Clear();
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
            if (txtNameProduct.Text != "" || txtPriceProduct.Text != "" || txtQuantityProduct.Text != "")
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
        List<Categorie> categoriesForProduct = new List<Categorie>();
        List<Categorie> categoriesInProduct = new List<Categorie>();
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

                btnCreateFacture.Visibility = Visibility.Visible;
                factures.Clear();
                lstFactureUser.Items.Clear();
                foreach (Facture facture in selectedUser.factures)
                {
                    factures.Add(facture);
                    lstFactureUser.Items.Add(facture.date);
                }
                #region client can't become prospect && 0 facture = can't become client
                if (client.users.GetStringType(selectedUser.type) == "Client")
                {
                    itemProspect.IsEnabled = false;
                }
                else
                {
                    itemProspect.IsEnabled = true;
                }

                if (factures == new List<Facture>())
                {
                    itemClient.IsEnabled = false;
                }
                else
                {
                    itemClient.IsEnabled = true;
                }
                #endregion
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
                        txtPasswordUser.Password,
                        factures
                    );
                    await client.users.Post(tmpUser);
                    refresh();
                }
            }
            else
            {
                lblWrong.Content = "Tous les champs ne sont pas remplis.";
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
                        txtPasswordUser.Password,
                        factures
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
        List<User> users = new List<User>();
        private void cboUserType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selected = (e.AddedItems[0] as ComboBoxItem).Content as string;
                if (selected == "Administrateur" || selected == "Employé")
                {
                    lblPassword.Visibility = Visibility.Visible;
                    txtPasswordUser.Visibility = Visibility.Visible;
                    lblFacture.Margin = new Thickness(1025, 100, 0, 0);
                    lstFactureUser.Margin = new Thickness(1025, 125, 0, 0);
                    lstFactureUser.Height = 200;

                    facture.Visibility = Visibility.Hidden;
                }
                else
                {
                    lblPassword.Visibility = Visibility.Hidden;
                    txtPasswordUser.Visibility = Visibility.Hidden;
                    txtPasswordUser.Password = "";
                    lblFacture.Margin = new Thickness(1025, 25, 0, 0);
                    lstFactureUser.Margin = new Thickness(1025, 50, 0, 0);
                    lstFactureUser.Height = 275;
                }
            }
            catch
            {
                hide();
            }
        }
        private void cboUserFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selected = (e.AddedItems[0] as ComboBoxItem).Content as string;
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
                    else if (selected == "Tous les types de compte")
                    {
                        lstUser.Items.Add(data);
                        users.Add(u);
                    }
                }
            }
            catch
            {
                hide();
            }
        }
        #endregion
        #region user factures
        List<Facture> factures = new List<Facture>();
        private void lstFactureUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstFactureUser.SelectedIndex >= 0)
            {
                facture.Visibility = Visibility.Visible;
                Facture selectedFacture = factures[lstFactureUser.SelectedIndex];
                txtIDFacture.Text = selectedFacture.id;
                txtDateFacture.Text = Convert.ToString(selectedFacture.date);
                txtClientFacture.Text = users[lstUser.SelectedIndex].name + " " + users[lstUser.SelectedIndex].surname;
                double total = 0;
                foreach(LigneFacture ligne in selectedFacture.lignes)
                {
                    NewLine();
                    textBox0.Text = ligne.product;
                    textBox1.Text = Convert.ToString(ligne.quantity);
                    textBox2.Text = Convert.ToString(ligne.price);
                    total += ligne.quantity*ligne.price;
                }
                txtTotalFacture.Text = Convert.ToString(total);
            }
        }
        private void btnCreateFacture_Click(object sender, RoutedEventArgs e)
        {
            lstFactureUser.SelectedItem = null;
            facture.Visibility = Visibility.Visible;
            txtIDFacture.Text = "";
            txtDateFacture.Text = "";
            txtClientFacture.Text = users[lstUser.SelectedIndex].name + " " + users[lstUser.SelectedIndex].surname;
            txtTotalFacture.Text = "";

            LigneFactureContainer.Children.Clear();
            nbLine = -1;
            gridHeight = 25;
            NewLine();
        }

        private void btnFactureValider_Click(object sender, RoutedEventArgs e)
        {
            factures.Add(new Facture(
                txtIDFacture.Text,
                Convert.ToDateTime(txtDateFacture.Text),
                lignes,
                txtClientFacture.Text
            ));
        }
        private void btnFactureDelete_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #region LigneFacture
        List<LigneFacture> lignes = new List<LigneFacture>();
        private int nbLine = -1;
        private int gridHeight = 25;
        private TextBox textBox0 = new TextBox();
        private TextBox textBox1 = new TextBox();
        private TextBox textBox2 = new TextBox();
        private Button button = new Button();
        private void NewLine()
        {
            nbLine++;
            LigneFactureContainer.Height = gridHeight;
            gridHeight += 35;

            Grid grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0,nbLine*35,0,0);
            grid.Width = 542;
            grid.Height = 25;
            LigneFactureContainer.Children.Add(grid);

            button = new Button();
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Width = 25;
            button.Height = 25;
            button.Content = "x";
            button.FontSize = 20;
            button.Padding = new Thickness(0,-8,0,0);
            grid.Children.Add(button);

            textBox0 = new TextBox();
            textBox0.HorizontalAlignment = HorizontalAlignment.Left;
            textBox0.Margin = new Thickness(35, 0, 0, 0);
            textBox0.Width = 150;
            textBox0.Height = 25;
            textBox0.FontSize = 15;
            textBox0.BorderBrush = new SolidColorBrush(Colors.White);
            grid.Children.Add(textBox0);

            Label label1 = new Label();
            label1.Width = 150;
            label1.Height = 25;
            label1.FontSize = 12;
            label1.Content = "Quantité :";
            grid.Children.Add(label1);

            textBox1 = new TextBox();
            textBox1.Margin = new Thickness(65,0,0,0);
            textBox1.Width = 85;
            textBox1.Height = 25;
            textBox1.FontSize = 15;
            textBox1.PreviewTextInput += IntValidationTextBox;
            grid.Children.Add(textBox1);

            Label label2 = new Label();
            label2.HorizontalAlignment = HorizontalAlignment.Right;
            label2.Width = 150;
            label2.Height = 25;
            label2.FontSize = 12;
            label2.Content = "Prix unitaire :";
            grid.Children.Add(label2);

            textBox2 = new TextBox();
            textBox2.HorizontalAlignment = HorizontalAlignment.Right;
            textBox2.Margin = new Thickness(50,0,0,0);
            textBox2.Width = 70;
            textBox2.Height = 25;
            textBox2.FontSize = 15;
            textBox1.PreviewTextInput += NumberValidationTextBox;
            grid.Children.Add(textBox2);
        }
        private void btnFactureNewLine_Click(object sender, RoutedEventArgs e)
        {
            NewLine();
            textBox0.Focus();
        }
        #endregion
        #endregion

        //categorie
        private void lstCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCategorie.SelectedIndex >= 0)
            {
                Categorie selectedCategorie = client.categories.cache[lstCategorie.SelectedIndex];

                txtIdCategorie.Text = selectedCategorie.id;
                txtNameCategorie.Text = selectedCategorie.name;
            }
            else
            {
                hide();
            }
        }
        private async void categorieAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameCategorie.Text != null)
            {
                Categorie tmpCategorie = new Categorie(
                    "",
                    txtNameCategorie.Text
                );
                await client.categories.Post(tmpCategorie);
                refresh();
            }
            else
            {
                lblWrong.Content = "Une catégorie à besoin d'un nom.";
            }
        }
        private async void categorieModifier_Click(object sender, RoutedEventArgs e)
        {
            if (txtIdCategorie.Text != null || txtNameCategorie.Text != null)
            {
                Categorie tmpCategorie = new Categorie(
                    txtIdCategorie.Text,
                    txtNameCategorie.Text
                );
                await client.categories.Put(tmpCategorie);
                refresh();
            }
            else
            {
                lblWrong.Content = "Il faut séléctionner un objet et lui donner un nom.";
            }
        }
        private async void categorieSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (txtIdCategorie.Text != null)
            {
                await client.categories.Delete(txtIdCategorie.Text);
                refresh();
            }
        }
        #region change grid
        private void goToCRUDCategorie_Click(object sender, RoutedEventArgs e)
        {
            gridListsCategorie.Visibility = Visibility.Hidden;
            gridCRUDCategorie.Visibility = Visibility.Visible;
        }
        private void goToCRUDProduit_Click(object sender, RoutedEventArgs e)
        {
            gridCRUDCategorie.Visibility = Visibility.Hidden;
            gridListsCategorie.Visibility = Visibility.Visible;
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
