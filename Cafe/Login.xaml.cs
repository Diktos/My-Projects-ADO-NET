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

namespace Cafe
{
 
    public partial class Login : BaseUserControl
    {
        public int SelectedUserId { get; set; }
        public event Action<int?, string> LoginResult;
        private int passwordCounter { get; set; } = 3;
        public Login()
        {
            InitializeComponent();
        }

        private void ThisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = Config.GetDbContext())
            {
                var users = context.Users.Select(s => new { s.Id, s.Name }).ToArray();
                userComboBox.ItemsSource = users;
                if (users.Length == 0)
                    throw new Exception("В базі відсутні офіціанти");
                userComboBox.SelectedValue = users[0].Id;

            }
        }
        private void CheckPassword()
        {
            using (var context = Config.GetDbContext())
            {
                var r = context.Users.SingleOrDefault(s => s.Id == SelectedUserId);
                if (r?.Password == PasswordBox.Password)
                {
                    LoginResult?.Invoke(r.Id, r.Name);
                }
                else
                {
                    passwordCounter--;
                    if (passwordCounter == 0)
                        LoginResult?.Invoke(null, string.Empty);
                    ErrorTextBlock.Text = $"Пароль введено неправильно, спробуйте ще {passwordCounter} раз.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                }
            }

        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;
        }
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CheckPassword();
        }


        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginResult?.Invoke(null, string.Empty);
        }
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }
    }
}

