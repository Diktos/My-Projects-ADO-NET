using System.Collections.ObjectModel;
using System.Data.SQLite;
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
using Cafe.Models; // Наша бібліотека класів
using Dapper;
using System.Collections; // Без юзінг не буде працюват Dapper

namespace Previous_DZ__but_with_using_Dapper
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Waiter> waiters { get; set; } = new ObservableCollection<Waiter>();
        Waiter SelectedWaiter = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (IDbConnection dbConnection = new SQLiteConnection("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db"))
            {
                dbConnection.Open();
                waiters.Clear();

                // Завантаження всіх записів у колекцію
                var result = dbConnection.Query<Waiter>("select* from Waiters");
               
                foreach (var waiter in result)
                {
                    waiters.Add(waiter);
                }

                dg.ItemsSource = waiters; // Зв'язує джерело даних (наприклад, waiters) з таблицею DataGrid
                dbConnection.Close();
            }
        }

        private void Button_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db"))
            {
                dbConnection.Open();

                foreach (var waiter in waiters)
                {
                    // Execute — це метод, який належить бібліотеці Dapper.
                    dbConnection.Execute("update Waiters set Name = @Name, Password = @Password, Birthday = @Birthday where Id = @Id",
                        // @ + ( CUSTOM name ) - параметри SQL запиту (@Name == waiter.Name)
                        waiter); 
                }

                dbConnection.Close();
            }
        }

        private void Button_AddRow_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Waiter newWaiter = new Waiter() // Можна і без "()" - конструктор викликається за замовчуванням без явного використання
                {
                    Name = NameTextBox.Text,
                    Password = PasswordTextBox.Text,
                    Birthday = DateTime.TryParse(BirthdayTextBox.Text, out DateTime result) ? result : DateTime.Now,
                };

                using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db"))
                {
                    dbConnection.Open();

                    dbConnection.Execute(
                        "insert into Waiters (Name, Password, Birthday) values (@Name, @Password, @Birthday)",
                        newWaiter);

                    // TODO - ще потрібно додати айді якось

                    // Присвоєння ID для нового запису, за найкращими умовами для бази даних
                    var lastWaiter = dbConnection.QuerySingle<Waiter>("SELECT* from Waiters ORDER BY Id DESC LIMIT 1");

                    newWaiter.Id = lastWaiter.Id;
                    lastWaiter = null;

                    waiters.Add(newWaiter);

                    dbConnection.Close();
                }
            }
        }

        private void Button_DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWaiter == null)
            {
                return;
            }

            using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db"))
            {
                dbConnection.Open();

                dbConnection.Execute("delete from Waiters where Id = @Id", new { SelectedWaiter.Id });

                dbConnection.Close();
            }

            waiters.Remove(SelectedWaiter);
            SelectedWaiter = null;
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedWaiter = dg.SelectedItem as Waiter;
        }
    }
}