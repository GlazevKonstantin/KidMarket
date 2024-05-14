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
    /// Логика взаимодействия для начальная.xaml
    /// </summary>
    public partial class StartWindow : Page
    {
        private string _login;
        private string _password;
        private string _role;
        
        public StartWindow()
        {
            InitializeComponent();
        }

        public void User(string login, string password, string role)
        {
            _login = login;
            _password = password;
            _role = role;
        }
        
        private void Click(object sender, RoutedEventArgs e)
        {
            Class1.frame.Navigate(new authentication());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            if (_role == "Менеджер")
            {
                Class1.frame.Navigate(new managerWindow(_login, _password));
                
            }
            else if (_role == "Админ")
            {
                Class1.frame.Navigate(new adminWindow(_login, _password));
            }
            else if (_role == "Пользователь")
            {
                Class1.frame.Navigate(new personalAccount(_login, _password));
                
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }

        private void Coaching_staff_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
            {
                // Получение всех пользователей из базы данных
                var sales = dbContext.Sales.ToList();

                // Привязка данных к DataGrid
                coaching_staff.ItemsSource = sales;
            }
        }

        private void Team_statistics_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
            {
                // Получение всех пользователей из базы данных
                var feedbacks = dbContext.Feedbacks.ToList();

                // Привязка данных к DataGrid
                team_statistics.ItemsSource = feedbacks;
            }
        }

        private void Sponsors_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
            {
                // Получение всех пользователей из базы данных
                var transportCompanies = dbContext.TransportCompanies.ToList();

                // Привязка данных к DataGrid
                transportCompanie.ItemsSource = transportCompanies;
            }
        }
    }
}
