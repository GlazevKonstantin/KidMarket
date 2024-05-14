using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Documents;
using System.Collections.Generic;

namespace KidMarket
{
    /// <summary>
    /// Логика взаимодействия для личный_кабинет.xaml
    /// </summary>
    public partial class personalAccount : Page
    {
        private string _login;
        private string _password;
        private int _ID;
        public personalAccount(string login, string password)
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
                var users = dbContext.Users;
                foreach (var user in users)
                {
                    if (user.UserLogin== _login && user.UserPassword == _password)
                    {
                        _ID = user.UserID;
                        txtFirstName.Text = user.UserName;
                        txtSurName.Text = user.UserSurname;
                        txtLastName.Text = user.UserLastName;
                        txtPhoneNumber.Text = user.UserPhoneNumber;
                        txtLogin.Text = user.UserLogin;
                        txtPassword.Text = user.UserPassword;
                        if (user.ImageData != null)
                        {
                            byte[] decodedBytes = user.ImageData;
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
        
        public class CartItem
        {
            public string Name { get; set; }

            public CartItem(string name)
            {
                Name = name;
            }
        }
        
        public ObservableCollection<CartItem> ShoppingCart { get; set; } = new ObservableCollection<CartItem>();

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string itemName = (button.Parent as StackPanel).Children.OfType<Label>().FirstOrDefault()?.Content.ToString();

                if (!string.IsNullOrEmpty(itemName))
                {
                    CartItem item = new CartItem(itemName);
                    ShoppingCart.Add(item);
                    ShoppingCartDataGrid.ItemsSource = null;
                    ShoppingCartDataGrid.ItemsSource = ShoppingCart;
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
                    var users = dbContext.Users;
                    foreach (var user in users)
                    {
                        if (user.UserLogin == _login && user.UserPassword == _password)
                        {
                            user.ImageData = imageBytes;
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
            
            string numberPhone = txtPhoneNumber.Text;
            if(numberPhone == "")
                errorMessage += "Номер телефона введен не корректно" + '\n';
            
            string login = txtLogin.Text;
            if(login == "")
                errorMessage += "Логин введен не корректно" + '\n';
            
            string password = txtPassword.Text;
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
                        var users = dbContext.Users;
                        foreach (var user in users)
                        {
                            if (user.UserLogin == _login && user.UserPassword == _password)
                            {
                                user.UserSurname = surname;
                                user.UserName = name;
                                user.UserLastName = lastName;
                                user.UserPhoneNumber = numberPhone;
                                user.UserLogin = login;
                                
                                user.UserPassword = password;
                                
                                
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
        
        private void Click(object sender, RoutedEventArgs e)
        {
            Class1.frame.Navigate(new StartWindow());
        }

        private void Btnagusha(object sender, RoutedEventArgs e)
        {
            BuyPrroduct("Детское питание");
        }

        private void Btnpris(object sender, RoutedEventArgs e)
        {
            BuyPrroduct("Присыпка детская");
        }

        private void Btnsoska(object sender, RoutedEventArgs e)
        {
            BuyPrroduct("Соска детская");
        }

        private void Btnpodguzn(object sender, RoutedEventArgs e)
        {
            BuyPrroduct("Подгузники детские");
        }

        void BuyPrroduct(string productName)
        {
            bool purchased = false;
            using (var dbContext = new MyDBContext())
            {
                dbContext.Database.CreateIfNotExists();
                var basket = dbContext.Basket;
                foreach (var product in basket)
                {
                    if (product.ProductName == productName && product.UserID== _ID)
                    {
                        purchased = true;
                        MessageBox.Show("Продукт уже в корзине");
                    }
                }

                if (!purchased)
                {
                    var newBasket = new Basket() { ProductName = productName, UserID = _ID };
                    dbContext.Basket.Add(newBasket);
                    dbContext.SaveChanges();
                    MessageBox.Show("Добавлено в корзину");
                }
            }
        }
        private void BtnFeedBack_Click(object sender, RoutedEventArgs e)
        {
            string feedBack = Txb_Feedback.Text;
            if (feedBack != "")
            {
                using (var dbContext = new MyDBContext())
                {
                    dbContext.Database.CreateIfNotExists();
                    var newFeedback = new Feedback() { Review = feedBack};
                    dbContext.Feedbacks.Add(newFeedback);
                    dbContext.SaveChanges();
                    MessageBox.Show("Отзыв успешно добавлен");
                }
            }
        }
        
        private void ShoppingCartDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            
            
            using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
            {
                // Получение всех пользователей из базы данных
                var basket = dbContext.Basket.ToList();
                List<Basket> myBasket = new List<Basket>();
                foreach (var a in basket)
                {
                    if(a.UserID == _ID)
                        myBasket.Add(a);
                }
                // Привязка данных к DataGrid
                ShoppingCartDataGrid.ItemsSource = myBasket;
            }
        }
            
        private void BtnBuyProduct_Click(object sender, RoutedEventArgs e) //BtnBuyProduct_Click
        {
            Basket selectedProduct = (Basket)ShoppingCartDataGrid.SelectedItem;
            if (selectedProduct != null)
            {
                using (var dbContext = new MyDBContext())
                {
                    dbContext.Database.CreateIfNotExists();
                    var users = dbContext.Users;
                    string userName = "";
                    string userSurName = "";
                    string userLastName = "";
                    string userEmail = "";
                    foreach (var user in users)
                    {
                        if (user.UserLogin == _login && user.UserPassword == _password)
                        {
                            userName = user.UserName;
                            userSurName = user.UserSurname;
                            userLastName = user.UserLastName;
                            userEmail = user.UserEmail;
                        }
                    }
                    
                    
                    var newOrder = new Order
                    {
                        UserName = userName, UserSurName = userSurName, UserLastName = userLastName,
                        UserEmail = userEmail, ProductName = selectedProduct.ProductName
                    };
                    dbContext.Orders.Add(newOrder);
                    dbContext.SaveChanges();
                    MessageBox.Show("Заказ совершен успешно");

                }
            }
        }
        private void Bacsket_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDBContext()) // Создание экземпляра контекста базы данных
            {
                var orders = dbContext.Orders.ToList();
                List<Order> myOrder = new List<Order>();
                string userName = "";
                string userSureName = "";
                string userLastName = "";
                string userEmail = "";

                var users = dbContext.Users;
                foreach (var user in users)
                {
                    if (user.UserID == _ID)
                    {
                        userName = user.UserName;
                        userSureName = user.UserSurname;
                        userLastName = user.UserLastName;
                        userEmail = user.UserEmail;
                    }
                }

                foreach (var order in orders)
                {
                    
                    if(order.UserName == userName && order.UserSurName == userSureName && order.UserLastName == userLastName && order.UserEmail == userEmail)
                        myOrder.Add(order);
                }
                // Привязка данных к DataGrid
                OrderDataGrid.ItemsSource = myOrder;
            }
            
        }
    }
}
