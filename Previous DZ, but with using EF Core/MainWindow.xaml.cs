using System.Collections.ObjectModel;
using System.Data;
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
using Cafe.Models;
using Microsoft.Data.Sqlite;
using CW_13._11_EF_Core;
using System.Data.Common;

namespace Previous_DZ__but_with_using_EF_Core
{
    public partial class MainWindow : Window
    {
        ObservableCollection<User> waiters { get; set; } = new ObservableCollection<User>();
        User SelectedWaiter = null;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new CafeDbContext())
            {
                MessageBox.Show("No users found in the database.", "1234", MessageBoxButton.OKCancel);
                waiters =new ObservableCollection<User>(context.Users.ToList());

                dg.ItemsSource = waiters; // Робимо зв'язок з DataGrid
            }
        }

        private void Button_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new CafeDbContext())
            {
                var currentWaiter = context.Users.FirstOrDefault(w => w.Id == SelectedWaiter.Id);  // Знаходимо існуючого офіціанта за ID

                if (currentWaiter != null)
                {
                    currentWaiter.Name = SelectedWaiter.Name;
                    currentWaiter.Password = SelectedWaiter.Password;
                    currentWaiter.Birthday = SelectedWaiter.Birthday;

                    context.SaveChanges();
                }
            }
        }
        
        private void Button_AddRow_Click(object sender, RoutedEventArgs e)           // Collapsed для 1 1 зробити    + все сховати
        {
            if (sender != null)
            {
                using (var context = new CafeDbContext())
                {
                    var newWaiter = new User
                    {
                        // Загрузка об"єкта даними з полей у ксамлі
                        Name = NameTextBox.Text, 
                        Password = PasswordTextBox.Text,
                        Birthday = DateTime.TryParse(BirthdayTextBox.Text, out DateTime result) ? result : DateTime.Now,
                    };

                    context.Users.Add(newWaiter); // Додаємо новий об'єкт до бази
                    context.SaveChanges();

                    waiters.Add(newWaiter); // Додаємо його в поточно-використовувану колекцію для dg.ItemsSource(DataGrid)
                }
            }
        }

        private void Button_DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWaiter == null)
            {
                return;
            }

            using (var context = new CafeDbContext())
            {
                var waiterToDelete = context.Users.SingleOrDefault<User>(w => w.Id == SelectedWaiter.Id); 

                if (waiterToDelete != null)
                {
                    context.Users.Remove(waiterToDelete); // Для бази
                    context.SaveChanges(); 

                    waiters.Remove(SelectedWaiter); // Для DataGrid
                    SelectedWaiter = null; // Для скидання селекшен об'єкт
                }
            }
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedWaiter = dg.SelectedItem as User;
        }
    }
}