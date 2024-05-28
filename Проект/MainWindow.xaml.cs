using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Проект
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private AuthenticationManager _authManager;

        public MainWindow()
        {
            InitializeComponent();

            //экземпляр AuthenticationManager
            _authManager = new AuthenticationManager("users.json");

            // Подпишиска на события
            _authManager.UserRegistered += AuthManager_UserRegistered;
            _authManager.AuthenticationFailed += AuthManager_AuthenticationFailed;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = boxLogin.Text;
            string password = boxPassword.Password;

            _authManager.RegisterUser(login, password);
        }

        private void AuthManager_AuthenticationFailed(object sender, string message)
        {
            txtStatus.Text = $"Регистрация неудалась: {message}";
        }

        private void AuthManager_UserRegistered(object sender, string username)
        {
            MainWin mainwIN = new MainWin();
            mainwIN.Show();
            Hide();
        }

        private void Button_Entry_Click(object sender, RoutedEventArgs e)
        {
            AuthwIN authwIN = new AuthwIN();
            authwIN.Show();
            Hide();
        }
    }
}