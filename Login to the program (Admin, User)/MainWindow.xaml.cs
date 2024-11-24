using Cafe.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Login_to_the_program__Admin__User_
{
    public partial class MainWindow : Window
    {
        public int attempts { get; set; } = 3;
        public ICommand commandToSing {  get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            commandToSing = new RelayCommand(Sign);
        }
        

        public void Sign()
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;

            using (var context = new CafeDbContext())
            {
                var user = context.Users.SingleOrDefault(u => u.Name == username && u.Password == password);
                if (user != null)
                {
                    var userRoles = context.UserRole
                    .Where(ur => ur.WaiterId == user.Id)
                    .Select(ur => ur.RoleId)
                    .ToArray();

                    if (userRoles.Any(roleId => roleId == 1))
                    {
                        ControlMenu starterMenu = new ControlMenu(); //Якщо адмін => виконається цей фрагмент коду
                        this.Close();
                        attempts = 3;

                        starterMenu.ShowDialog();
                    }
                    else
                    {
                        Application.Current.Shutdown(); // Якщо він не адмін, він не має можливості змінювати дані
                                                        // (у нашому випадку, тестовому, просто не відкриваємо йому вікно з доступом)
                    }
                }
                else
                {
                    // Невдача у вході
                    if (attempts <= 0)
                    {
                        Application.Current.Shutdown();
                    }
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    ErrorTextBlock.Text = $"Invalid login. Attempts left: {attempts}";
                    attempts--;
                }
            }
        }
    }
}