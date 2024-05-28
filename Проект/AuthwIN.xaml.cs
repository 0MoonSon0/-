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

namespace Проект
{
    /// <summary>
    /// Логика взаимодействия для AuthwIN.xaml
    /// </summary>
    public partial class AuthwIN : Window
    {
        private AuthenticationManager _authManager;
        public AuthwIN()
        {
            InitializeComponent();

            //экземпляр AuthenticationManager
            _authManager = new AuthenticationManager("users.json");

            // Подписка на события
            _authManager.UserAuthenticated += AuthManager_UserAuthenticated;            
            _authManager.AuthenticationFailed += AuthManager_AuthenticationFailed;
        }
        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = boxLogin.Text;
            string password = boxPassword.Password;

            _authManager.AuthenticateUser(login, password);

        }
        private void AuthManager_UserAuthenticated(object sender, string username)
        {
            MainWin mainWin = new MainWin();
            mainWin.Show();
            Hide();
        }

        private void AuthManager_AuthenticationFailed(object sender, string message)
        {
            txtStatus.Text = $"Авторизация провалилась: {message}";
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {            
            MainWindow regWin = new MainWindow();
            regWin.Show();
            Hide();
        }

        private void boxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }         
}
    

