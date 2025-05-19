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

namespace Travel_agency_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ServerConnection sc = new ServerConnection();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            StatusText.Text = "Regisztráció folyamatban";

            bool success = await sc.Registration(UsernameBox.Text, PasswordBox.Password);


            if (success)
            {
                StatusText.Text = "Sikeres regisztráció";
            }
            else
            {
                StatusText.Text = "Sikertelenes regisztráció ";
                UsernameBox.Text = "";
                PasswordBox.Password = "";
            }
        }

        private async void LoginB_Click(object sender, RoutedEventArgs e)
        {

            StatusText.Text = "Bejelentkezés folyamatban";

            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            string token = await sc.LoginAsync(username, password);

            if (token == "Error")
            {
                StatusText.Text = "Sikertelen Bejelentkezés";

            }
            else if (token == "NTE")
            {
                StatusText.Text = "Hiba a bejelentkezés folyamán, ellenőrizze a kapcsolatot, és próbálkozzon újra";
            }
            else
            {
                StatusText.Text = "Sikeres Bejelentkezés";
                Travel_agency_idk_mainwindow mw = new Travel_agency_idk_mainwindow(token);
                mw.Closed += (s2, e2) => this.Close();
                mw.Show();
                this.Hide();
            }
        }
    }
}
