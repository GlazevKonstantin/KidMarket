using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace KidMarket
{
    /// <summary>
    /// Логика взаимодействия для регистрация.xaml
    /// </summary>
    public partial class registrationWindow : Page
    {
        public registrationWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            string surname = TxbSurname.Text;
            if (surname == "" && surname == " ")
                errorMessage += "Фамилия введена не корректно" + '\n';
            
            string name = TxbName.Text;
            if(name == "")
                errorMessage += "Имя введено не корректно" + '\n';
            
            string lastName = TxbLastName.Text;
            if(lastName == "")
                errorMessage += "Отчество введено не корректно" + '\n';
            
            string numberPhone = TxbPhoneNumber.Text;
            if(numberPhone == "")
                errorMessage += "Номер телефона введен не корректно" + '\n';
            
            string email = TxbEmail.Text;
            if(email == "")
                errorMessage += "Емаил введен не корректно" + '\n';
            
            string login = TxbLogin.Text;
            if(login == "")
                errorMessage += "Логин введен не корректно" + '\n';
            
            string password = PwbPassword.Text;
            if(password == "")
                errorMessage += "Пароль введен не корректно" + '\n';
            
            if (errorMessage == "")
            {
                using (var dbContext = new MyDBContext())
                {
                    dbContext.Database.CreateIfNotExists();
                    var users = dbContext.UsersAll;
                    bool recuringData = false;
                    foreach (var user in users)
                    {
                        if (user.Login == login)
                        {
                            recuringData = true;
                        }
                    }

                    if (!recuringData)
                    {
                        var newUser = new User
                        {
                            UserSurname = surname, UserName = name, UserLastName = lastName,
                            UserPhoneNumber = numberPhone, UserEmail = email, UserLogin = login, UserPassword = password
                        };
                        dbContext.Users.Add(newUser);
                        var newUserAll = new UserAll() { Login = login, Password = password, Role = "Пользователь" };
                        dbContext.UsersAll.Add(newUserAll);
                        dbContext.SaveChanges();
                        MessageBox.Show("Пользователь успешно добавлен");
                        Class1.frame.Navigate(new authentication());
                    }

                    else
                        MessageBox.Show("Пользователь с таким логином уже существует");
                }
            }
            else
                MessageBox.Show(errorMessage);

        }

        private void AdminReg_Click(object sender, RoutedEventArgs e)
        {
            AdminOrManagerAdd adminAdd = new AdminOrManagerAdd("Admin");
            Class1.frame.Navigate(adminAdd);
        }

        private void ManagerReg_Click(object sender, RoutedEventArgs e)
        {
            AdminOrManagerAdd managerAdd = new AdminOrManagerAdd("Manager");
            Class1.frame.Navigate(managerAdd);
        }
    }
}
