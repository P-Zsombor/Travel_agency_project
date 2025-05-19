using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace Travel_agency_project
{
    /// <summary>
    /// Interaction logic for Travel_agency_idk_mainwindow.xaml
    /// </summary>
    public partial class Travel_agency_idk_mainwindow : Window
    {
        string tokenSaver;

        ServerConnection sc = new ServerConnection();

        public Travel_agency_idk_mainwindow(string token)
        {
            InitializeComponent();
            tokenSaver = token;
            HideEverithing();
        }

        // Profil megtekintése
        private async void btnGetProfile_Click(object sender, RoutedEventArgs e)
        {
            string adat = await sc.GetProfileDataAsync(tokenSaver);

            if (adat.StartsWith("Hiba"))
            {
                nameTB.Text = adat.ToString();
                return;
            }

            try
            {
                var profil = JsonConvert.DeserializeObject<JsonResponse>(adat);
                if (profil != null)
                {

                    nameTB.Text = "Profil name: " +  profil.username;
                    passwordTB.Text = "Profile Password: " + profil.password;
                }
                else
                {
                    MessageBox.Show("sikertelen");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("hiba:" + err.Message);
            }

            HideEverithing();
            ShowProfile();


        }

        // Profil megjelenítése
        void ShowProfile()
        {
            AccountDetailsSP.Visibility = Visibility.Visible;

        }


        void HideEverithing()               //contentből mindent hidol
        {
            AccountDetailsSP.Visibility = Visibility.Hidden;
            UpdateTripSP.Visibility = Visibility.Hidden;
            DeleteTripSP.Visibility = Visibility.Hidden;
            SearchTripSP.Visibility = Visibility.Hidden;
        }

        // Utazás módosításának megjelenítése
        void ShowUpdate(object s, RoutedEventArgs e)
        {
            HideEverithing();
            UpdateTripSP.Visibility = Visibility.Visible;
        }

        // Utazás törlésének megjelenítése
        void ShowDelete(object s, RoutedEventArgs e)
        {
            HideEverithing();
            DeleteTripSP.Visibility = Visibility.Visible;
        }

        // Utazás keresésének megjelenítése
        void ShowSearch(object s, RoutedEventArgs e)
        {
            HideEverithing();
            SearchTripSP.Visibility = Visibility.Visible;
        }

        // Utazás módosítása
        async void UpdateTrip(object s, RoutedEventArgs e)
        {
            if (await sc.UpdateTrip(Convert.ToInt32(updateIdTB.Text), updateNameTB.Text, updateDestinationTB.Text, updateAccommodationTB.Text, updateTransportTB.Text))
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        // Utazás törlése
        async void DeleteTrip(object s, RoutedEventArgs e)
        {
            if (await sc.DeleteTrip(Convert.ToInt32(deleteIdTB.Text)))
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        // Utazás keresése név alapján
        async void SearchTrip(object s, RoutedEventArgs e)
        {
            if ((await sc.SearchName(searchNameTB.Text)) != null)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
