using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для админ.xaml
    /// </summary>
    public partial class adminWindow : Page
    {
        private string _login;
        private string _password;
        public adminWindow(string login, string password)
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
                var admins = dbContext.Admins;
                foreach (var admin in admins)
                {
                    if (admin.AdminLogin == _login && admin.AdminPassword == _password)
                    {
                        txtFirstName.Text = admin.AdminName;
                        txtSurName.Text = admin.AdminSurname;
                        txtLastName.Text = admin.AdminLastName;
                        txbLogin.Text = admin.AdminLogin;
                        txbPassword.Text = admin.AdminPassword;
                        if (admin.ImageData != null)
                        {
                            byte[] decodedBytes = admin.ImageData;
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

        private void BtnAddTransportCompany_Click(object sender, RoutedEventArgs e)
        {
            string transportCompanyName = Txb_TK.Text;
            string deliveryTime = Txb_Term.Text;
            string termsOfDelivery = Txb_Conditions.Text;
            bool repeat = false;
            if (transportCompanyName != "" && deliveryTime != "" && termsOfDelivery != "")
            {
                using (var dbContext = new MyDBContext())
                {
                    var transportCompanys = dbContext.TransportCompanies;
                    foreach (var transportCompany in transportCompanys)
                    {
                        if (transportCompany.TransportCompanyName == transportCompanyName)
                        {
                            repeat = true;
                        }
                    }

                    if (!repeat)
                    {
                        var newTransportCompany = new TransportCompany()
                        {
                            TransportCompanyName = transportCompanyName, DeliveryTime = deliveryTime,
                            TermsOfDelivery = termsOfDelivery
                        };
                        dbContext.TransportCompanies.Add(newTransportCompany);
                        dbContext.SaveChanges();
                        MessageBox.Show("ТК успешно добавлена");
                    }
                    else
                    {
                        MessageBox.Show("ТК с таким названием уже добавлена");
                    }
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
        }
        
        private void BtnAddSale_Click(object sender, RoutedEventArgs e)
        {
            string discountTerms = TxbСonditions.Text;
            string discountAmount = TxbSize.Text;
            string startDate = TxbDate_1.Text;
            string endDate = TxbDate_2.Text;
            if (discountTerms != "" && discountAmount != "" && startDate != "" && endDate != "")
            {
                using (var dbContext = new MyDBContext())
                {
                    var newSale = new Sale
                    {
                        DiscountTerms = discountTerms, DiscountAmount = discountAmount, StartDate = startDate,
                        EndDate = endDate
                    };
                    dbContext.Sales.Add(newSale);
                    dbContext.SaveChanges();
                    MessageBox.Show("Скидка добавлена успешно");
                }
            }
            
            else
            {
                MessageBox.Show("Не все поля заполнены");
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
                    var admins = dbContext.Admins;
                    foreach (var admin in admins)
                    {
                        if (admin.AdminLogin == _login && admin.AdminPassword == _password)
                        {
                            admin.ImageData = imageBytes;
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
                        var admins = dbContext.Admins;
                        foreach (var admin in admins)
                        {
                            if (admin.AdminLogin == _login && admin.AdminPassword == _password)
                            {
                                admin.AdminSurname = surname;
                                admin.AdminName = name;
                                admin.AdminLastName = lastName;
                                admin.AdminLogin = login;
                                admin.AdminPassword = password;
                                
                                
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
    }
    
    
}
