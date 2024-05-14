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
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace KidMarket
{
    /// <summary>
    /// Логика взаимодействия для менеджер.xaml
    /// </summary>
    public partial class managerWindow : Page
    {
        private string _login;
        private string _password;
        public managerWindow(string login, string password)
        {
            InitializeComponent();
            _login = login;
            _password = password;
            FillingInData();
        }

        void FillingInData()
        {
            using (var dbContext = new MyDBContext())
            {
                dbContext.Database.CreateIfNotExists();
                var managers = dbContext.Managers;
                foreach (var manager in managers)
                {
                    if (manager.ManagerLogin == _login && manager.ManagerPassword == _password)
                    {
                        txtFirstName.Text = manager.ManagerName;
                        txtSurName.Text = manager.ManagerSurname;
                        txtLastName.Text = manager.ManagerLastName;
                        txbLogin.Text = manager.ManagerLogin;
                        txbPassword.Text = manager.ManagerPassword;
                        if (manager.ImageData != null)
                        {
                            byte[] decodedBytes = manager.ImageData;
                            // Создание изображения из массива байтов
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = new MemoryStream(decodedBytes);
                            bitmapImage.EndInit();
                            ProfileImage.Source = bitmapImage;
                        }
                    }
                }
                
            }
        }
        
        private void BtnReg(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                byte[] imageBytes = File.ReadAllBytes(selectedImagePath);
                using (var dbContext = new MyDBContext())
                {
                    var managers = dbContext.Managers;
                    foreach (var manager in managers)
                    {
                        if (manager.ManagerLogin == _login && manager.ManagerPassword == _password)
                        {
                            manager.ImageData = imageBytes;
                        }
                    }
                    dbContext.SaveChanges();
                }
                ProfileImage.Source = new BitmapImage(new Uri(selectedImagePath));
                
            }
        }

        private void SavePersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            string surname = txtSurName.Text;
            if (surname == "" && surname == " ")
                errorMessage += "Фамилия введена не корректно" + '\n';
            
            string name = txtFirstName.Text;
            if(name == "")
                errorMessage += "Имя введено не корректно" + '\n';
            
            string lastName = txtLastName.Text;
            if(lastName == "")
                errorMessage += "Отчество введено не корректно" + '\n';
            
            
            string login = txbLogin.Text;
            if(login == "")
                errorMessage += "Логин введен не корректно" + '\n';
            
            string password = txbPassword.Text;
            if(password == "")
                errorMessage += "Пароль введен не корректно" + '\n';
            
            if (errorMessage == "")
            {
                using (var dbContext = new MyDBContext())
                {
                    dbContext.Database.CreateIfNotExists();
                    var usersAll = dbContext.UsersAll;
                    bool recuringData = false;
                    
                    foreach (var user in usersAll)
                    {
                        if (_login != login)
                        {
                            if (user.Login == login)
                            {
                                recuringData = true;
                            }
                        }
                    }

                    if (!recuringData)
                    {
                        var managers = dbContext.Managers;
                        foreach (var manager in managers)
                        {
                            if (manager.ManagerLogin == _login && manager.ManagerPassword == _password)
                            {
                                manager.ManagerSurname = surname;
                                manager.ManagerName = name;
                                manager.ManagerLastName = lastName;
                                manager.ManagerLogin = login;
                                manager.ManagerPassword = password;
                            }
                        }
                        
                        foreach (var userAll in usersAll)
                        {
                            if (userAll.Login == _login && userAll.Password == _password)
                            {
                                userAll.Login = login;
                                userAll.Password = password;
                                _login = login;
                                _password = password;
                            }
                        }
                        dbContext.SaveChanges();
                    }

                    else
                        MessageBox.Show("Пользователь с таким логином уже существует");
                }
            }
            else
                MessageBox.Show(errorMessage);
        }

        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            Class1.frame.Navigate(new registrationWindow());
        }

        private void Change_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Remove_Btn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (UserAll)UserGrid.SelectedItem;

            if (selectedUser != null)
            {
                string login = selectedUser.Login;
                string password = selectedUser.Password;
                string role = selectedUser.Role;
                if (login != _login && password != _password && role != "Менеджер")
                {
                    using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
                    {
                        var usersAll = dbContext.UsersAll;
                        foreach (var userAll in usersAll)
                        {
                            if (userAll.Login == login && userAll.Password == password)
                            {
                                dbContext.UsersAll.Remove(userAll);
                            }
                        }

                        if (role == "Админ")
                        {
                            var admins = dbContext.Admins;
                            foreach (var admin in admins)
                            {
                                if (admin.AdminLogin == login && admin.AdminPassword == password)
                                {
                                    dbContext.Admins.Remove(admin);
                                }
                            }

                            dbContext.SaveChanges();

                        }
                        else if (role == "Менеджер")
                        {
                            var managers = dbContext.Managers;
                            foreach (var manager in managers)
                            {
                                if (manager.ManagerLogin == login && manager.ManagerPassword == password)
                                {

                                    dbContext.Managers.Remove(manager);
                                }
                            }

                            dbContext.SaveChanges();
                        }
                        else if (role == "Пользователь")
                        {
                            var users = dbContext.Users;
                            foreach (var user in users)
                            {
                                if (user.UserLogin == login && user.UserPassword == password)
                                {
                                    dbContext.Users.Remove(user);
                                }
                            }

                            dbContext.SaveChanges();
                        }

                        var usersAllCont = dbContext.UsersAll.ToList();
                        UserGrid.ItemsSource = usersAllCont;
                    }



                }
                else
                    MessageBox.Show("Нельзя удалить свою учетную запись");
            }
        }

        private void UserGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
            {
                // Получение всех пользователей из базы данных
                var usersAll = dbContext.UsersAll.ToList();

                // Привязка данных к DataGrid
                UserGrid.ItemsSource = usersAll;
            }
        }

        private void OrderGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
            {
                // Получение всех пользователей из базы данных
                var orders = dbContext.Orders.ToList();

                // Привязка данных к DataGrid
                OrderGrid.ItemsSource = orders;
            }
        }
    }
}
