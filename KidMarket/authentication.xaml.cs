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

namespace KidMarket
{
    /// <summary>
    /// Логика взаимодействия для вход.xaml
    /// </summary>
    public partial class authentication : Page
    {
        public authentication()
        {
            InitializeComponent();
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            string login = TxbLogin.Text;
            string password = TxbPassword.Text;
            bool succes = false;
            using (var dbContext = new MyDBContext())
            {
                dbContext.Database.CreateIfNotExists();
                var usersAll = dbContext.UsersAll;
                foreach (var user in usersAll)
                {
                    if (user.Login == login && user.Password == password)
                    {
                        StartWindow startWindow = new StartWindow();
                        startWindow.User(user.Login, user.Password, user.Role);
                        Class1.frame.Navigate(startWindow);
                        succes = true;
                    }
                }

                if (!succes)
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }

        private void BtnReg(object sender, RoutedEventArgs e)
        {
            Class1.frame.Navigate(new registrationWindow());
        }
    }
}
