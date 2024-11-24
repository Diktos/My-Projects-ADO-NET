using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Cafe.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Login_to_the_program__Admin__User_
{
    public partial class RightsEditWindow : Window
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public int SelectedObjectId { get; set; } = -1;

        public RightsEditWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ObservableCollection<RoleWithIsChecked> TypeRoles { get; set; } = new ObservableCollection<RoleWithIsChecked>
        {
            new RoleWithIsChecked { Role = Role.User, IsChecked = false },
            new RoleWithIsChecked { Role = Role.Manager, IsChecked = false },
            new RoleWithIsChecked { Role = Role.Admin, IsChecked = false }
        };


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new CafeDbContext())
            {
                Users = new ObservableCollection<User>(context.Users.ToList());
                UserDataGrid.ItemsSource = Users;
            }
        }

        private void dg_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID" || e.Column.Header.ToString() == "Name" || e.Column.Header.ToString() == "Birthday") // Перевіряємо, чи стовпець - Id 
            {
                e.Cancel = true; // Забороняємо редагування
            }
        }
        private void UserDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedObjectId = (UserDataGrid.SelectedItem as User).Id;

            // Новий користувач, тому оновлюємо його плюсики у комбобокс
            if (SelectedObjectId != null)
            {
                using (var context = new CafeDbContext())
                {
                    var userRoles = context.UserRole
                   .Where(ur => ur.WaiterId == SelectedObjectId)
                   .Select(ur => ur.RoleId)
                   .ToList();

                    for (int i = 0; i < TypeRoles.Count; i++) // у foreach не можна змінювати айтем напряму
                    {
                        if (userRoles.Contains(TypeRoles[i].Role.Id))
                        {
                            TypeRoles[i].IsChecked = true;
                        }
                        else if (!userRoles.Contains(TypeRoles[i].Role.Id))
                        {
                            TypeRoles[i].IsChecked = false;
                        }
                    }
                }
            }
        }


        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Додаємо чи видаляємо роль
        {
            if (
                (RoleComboBox.SelectedItem as RoleWithIsChecked == null || SelectedObjectId==-1)
                || (RoleComboBox.SelectedItem as RoleWithIsChecked == null && SelectedObjectId == -1)
                )
            {
                return;
            }

            var selectedRoleId = (RoleComboBox.SelectedItem as RoleWithIsChecked).Role.Id;
           
            using (var context = new CafeDbContext())
            {
                // Отримуємо список існуючих ролей для вибраного користувача
                var userRoles = context.UserRole
                    .Where(ur => ur.WaiterId == SelectedObjectId)
                    .Select(ur => ur.RoleId)
                    .ToList();

                // Перебираємо всі доступні ролі
               

                if (userRoles.Contains(selectedRoleId))
                {
                    // Якщо роль існує, то видаляємо
                    var userRoleToRemove = context.UserRole
                        .SingleOrDefault(ur => ur.WaiterId == SelectedObjectId && ur.RoleId == selectedRoleId);
                    var i = context.UserRole
                           .Count(ur => ur.RoleId == Role.Admin.Id);
                    if (userRoleToRemove != null)
                    {

                        if (selectedRoleId == Role.Admin.Id && context.UserRole
                           .Count(ur => ur.RoleId == Role.Admin.Id) <= 1)
                        {
                            MessageBox.Show("You can't delete the last user with Admin rights","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                            RoleComboBox.SelectedItem = null; // Фікс проблеми з махінаціями у виборі ролі
                            return;
                        }
                        context.UserRole.Remove(userRoleToRemove);

                        TypeRoles.SingleOrDefault(ur=>ur.Role.Id==selectedRoleId).IsChecked = false; // Змінюємо статус у UI для RoleComboBox
                    }
                }
                else
                {
                    // Якщо ролі немає, то додаємо
                    context.UserRole.Add(new UserRole { WaiterId = SelectedObjectId, RoleId = selectedRoleId });

                    TypeRoles.SingleOrDefault(ur => ur.Role.Id == selectedRoleId).IsChecked = true; // Змінюємо статус у UI для RoleComboBox
                }
                
                context.SaveChanges();
            }
            // Встановлюємо значення для ComboBox, щоб показати ваш текст.
            RoleComboBox.SelectedItem = null; // Фікс проблеми з махінаціями у виборі ролі
        }
    }
}
