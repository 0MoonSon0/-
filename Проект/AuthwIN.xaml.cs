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

            // Создаем экземпляр AuthenticationManager, передаем путь к файлу с пользователями
            _authManager = new AuthenticationManager("users.json");

            // Подписка на события AuthenticationManager
            _authManager.UserAuthenticated += AuthManager_UserAuthenticated;            
            _authManager.AuthenticationFailed += AuthManager_AuthenticationFailed;
        }
        //кнопка авторизации
        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = boxLogin.Text;
            string password = boxPassword.Password;

            // Попытка аутентификации пользователя
            _authManager.AuthenticateUser(login, password);

        }
        private void AuthManager_UserAuthenticated(object sender, string username)
        {
            //успешная авторизация, переход в маин вин
            MainWin mainWin = new MainWin();
            mainWin.Show();
            Hide();
        }

        private void AuthManager_AuthenticationFailed(object sender, string message)
        {
            // Обработка ошибки аутентификации
            txtStatus.Text = $"Авторизация провалилась: {message}";
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            // переход в окно регистрации
            MainWindow regWin = new MainWindow();
            regWin.Show();
            Hide();
        }

        private void boxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }         
}
    

