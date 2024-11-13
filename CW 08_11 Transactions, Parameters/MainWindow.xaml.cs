using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
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

namespace CW_08_11_Transactions__Parameters
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
            using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db"))
            {
                dbConnection.Open();
                waiters.Clear();

                dg.ItemsSource = waiters;

                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "select* from Waiters";
                var dbReader = dbCommand.ExecuteReader();

                while (dbReader.Read()) // Заповнюєм масив
                {
                    waiters.Add(new Waiter { Id = dbReader.GetInt32(0), Name = dbReader.GetString(1), Password = dbReader.GetString(2), Birthday = dbReader.GetDateTime(3) });
                }

                dbConnection.Close();
            }
        }

        private void Button_SaveChanges_Click(object sender, RoutedEventArgs e) // Update record
        {
            using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db"))
            {
                dbConnection.Open();

                foreach (var waiter in waiters)
                {
                        var dbCommand = dbConnection.CreateCommand();
                        dbCommand.CommandText = "update Waiters set Name = @par_Name, Password = @par_Password, Birthday = @par_Birthday where Id = @par_Id";

                        var parName = dbCommand.CreateParameter();
                        parName.ParameterName = "@par_Name";
                        parName.Value = waiter.Name;
                        dbCommand.Parameters.Add(parName);

                        dbCommand.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@par_Password", waiter.Password));
                        dbCommand.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@par_Birthday", waiter.Birthday));
                        dbCommand.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@par_Id", waiter.Id)); // Потрібно для порівняння 

                        dbCommand.ExecuteNonQuery(); // Виконання SQL-запита
                }
                dbConnection.Close();
            }
        }

        private void Button_AddRow_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Waiter newWaiter = new Waiter()
                {
                    Name = NameTextBox.Text,
                    Password = PasswordTextBox.Text,
                    Birthday = DateTime.TryParse(BirthdayTextBox.Text, out DateTime result) ? DateTime.Parse(BirthdayTextBox.Text) : DateTime.Now,
                };

                using (IDbConnection dbConnection = new System.Data.SQLite.SQLiteConnection("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db"))
                {
                    dbConnection.Open();

                    var dbCommand = dbConnection.CreateCommand();
                    dbCommand.CommandText = "insert into Waiters (Name, Password, Birthday) values (@par_Name, @par_Password, @par_Birthday)";


                    dbCommand.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@par_Name", newWaiter.Name));
                    dbCommand.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@par_Password", newWaiter.Password));
                    dbCommand.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@par_Birthday", newWaiter.Birthday));

                    dbCommand.ExecuteNonQuery();


                    dbCommand.CommandText = "select last_insert_rowid()";  // Запит для отримання останнього ID
                    using (var reader = dbCommand.ExecuteReader())  
                    {
                        if (reader.Read())
                        {
                            newWaiter.Id = int.Parse(reader[0].ToString()); 
                        }
                    }
    
                dbConnection.Close();
                }
                
                waiters.Add(newWaiter);
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

                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "delete from Waiters where Id = @par_Id";

                dbCommand.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@par_Id", SelectedWaiter.Id));

                if (SelectedWaiter != null)
                {
                    waiters.Remove(SelectedWaiter);
                }
                SelectedWaiter = null;

                dbCommand.ExecuteNonQuery();

                dbConnection.Close();
            }
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedWaiter = dg.SelectedItem as Waiter;
        }
    }
}