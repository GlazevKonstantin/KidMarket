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

namespace KidMarket
{
    /// <summary>
    /// Логика взаимодействия для AdminAdd.xaml
    /// </summary>
    
    public partial class AdminOrManagerAdd : Page
    {
        public AdminOrManagerAdd(string role)
        {
            InitializeComponent();
            _role = role;
            if (role == "Admin")
                RoleText.Content = "Админ";
            else if (role == "Manager")
                RoleText.Content = "Менеджер";
        }

        private string _role;
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
           string errorMessage = "";
            string surname = TxbSurname.Text;
            if (surname == "" || surname == " ")
                errorMessage += "Фамилия введена не корректно" + '\n';
            
            string name = TxbName.Text;
            if(name == "" ||  name == " ")
                errorMessage += "Имя введено не корректно" + '\n';
            
            string lastName = TxbLastName.Text;
            if(lastName == "" ||  lastName == " ")
                errorMessage += "Отчество введено не корректно" + '\n';
            
            string login = TxbLogin.Text;
            if(login == "" ||  login == " ")
                errorMessage += "Логин введен не корректно" + '\n';
            
            string password = PwbPassword.Text;
            if(password == "" ||  password == " ")
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
                        string role = "";
                        if (_role == "Admin")
                        {
                            role = "Админ";
                            var newAdmin = new Admin() { AdminSurname = surname, AdminName = name, AdminLastName = lastName, AdminLogin = login, AdminPassword = password };
                            dbContext.Admins.Add(newAdmin);
                            var newUser = new UserAll() { Login = login, Password = password, Role = role };
                            dbContext.UsersAll.Add(newUser);
                        }
                        
                        else if (_role == "Manager")
                        {
                            role = "Менеджер";
                            var newManager = new Manager() { ManagerSurname = surname,ManagerName = name, ManagerLastName = lastName, ManagerLogin = login, ManagerPassword = password };
                            dbContext.Managers.Add(newManager);
                            var newUser = new UserAll() { Login = login, Password = password, Role = role };
                            dbContext.UsersAll.Add(newUser);
                        }
                        dbContext.SaveChanges();
                        MessageBox.Show(role+" успешно добавлен");
                        Class1.frame.Navigate(new authentication());
                    }

                    else
                        MessageBox.Show("Пользователь с таким логином уже существует");
                }
            }
            else
                MessageBox.Show(errorMessage);
        }
    }
}
