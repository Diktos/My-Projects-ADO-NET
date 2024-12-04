using Cafe.Models;
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
    public partial class NomenclatureItems : UserControl
    {
        public event Action<int?> EditItem;
        public event Action<int?> RemoveItem;
        public event Action CloseWindow;
        public Nomenclature SelectedNomenclature { get; set; }
        public NomenclatureItems()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            using (var dbContext = Config.GetDbContext())
            {
                dGrid.ItemsSource = dbContext.Nomenclatures.ToArray();
            }
        }

        private void dGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((Nomenclature)dGrid.CurrentItem)!=null)
            { 
                EditItem?.Invoke(((Nomenclature)dGrid.CurrentItem).Id);
            }
        }

        private void RemoveNomenclature_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNomenclature != null)
            {
                RemoveItem?.Invoke(SelectedNomenclature.Id);
            }
            else
            {
                RemoveItem?.Invoke(null);
            }
        }

        private void AddNomenclature_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNomenclature == null)
            {
                EditItem?.Invoke(null);
            }
        }

        public void RemoveItemById(int id)
        {
            using(var dbContext = Config.GetDbContext())
            {
                var i = dbContext.Nomenclatures.SingleOrDefault(n=>n.Id==id);
                dbContext.Nomenclatures.Remove(i);
                dbContext.SaveChanges(); // Не забуваємо про збреження змін!
            }
        }

        private void dGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedNomenclature = dGrid.SelectedItem as Nomenclature;
        }

        private void FindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchRequest = FindTextBox.Text; // Отримуємо введений текст 

            using (var dbContext = Config.GetDbContext())
            {
                var items = dbContext.Nomenclatures.ToList();

                // Фільтруємо елементи за назвою 
                var filteredItems = items.Where(item => item.Name.Contains(searchRequest)).ToList();

                // Оновлюємо джерело даних DataGrid
                dGrid.ItemsSource = filteredItems;
            }
        }

        private void PrevMenuButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow.Invoke();
        }
    }
}
