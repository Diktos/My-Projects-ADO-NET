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
    public partial class OrderItems : UserControl
    {
        public event Action<int?> EditOrder;
        public event Action<int?> Remove_Order;
        public event Action CloseWindow;
        public Order SelectedOrder { get; set; }

        public OrderItems()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            using (var context = Config.GetDbContext())
            {
                var data = context.Orders
                    .Where(s => s.UserId == Config.UserId)
                    .Join(context.ClientTables,
                        ws => ws.ClientTableId,
                        ab => ab.Id,
                        (ws, ab) => new
                        {
                            Id = ws.Id,
                            Abonent = ab.Name,
                            Time_order = ws.Date.ToString("H:mm:ss"),
                            Summ = context.OrderDetails
                                .Where(w => w.OrderId == ws.Id)
                                .Sum(sl => sl.Sum),
                            Bill = ws.Bill
                        }).ToArray();

                dGrid.ItemsSource = data;
            }
        }

        private void dGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dGrid.CurrentItem is not null)
            {
                EditOrder?.Invoke(((dynamic)dGrid.CurrentItem).Id);
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            EditOrder?.Invoke(null);
        }

        private void RemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder != null)
            {
                Remove_Order?.Invoke(SelectedOrder.Id);
            }
            else
            {
                Remove_Order?.Invoke(null);
            }
        }

        public void RemoveOrderById(int id)
        {                                                              
            using (var context = Config.GetDbContext())
            {
                var order = context.Orders.SingleOrDefault(o => o.Id == id);
                if (order != null)
                {
                    context.Orders.Remove(order);
                    context.SaveChanges(); // Збереження змін
                }
            }
        }

        private void dGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedOrder = dGrid.SelectedItem as Order;
        }

        private void PrevMenuButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow?.Invoke();
        }
    }

}
