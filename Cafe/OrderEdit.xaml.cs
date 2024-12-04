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
    public partial class OrderEdit : UserControl
    {
        public int? Id { get; set; }

        public event Action<bool, string?> Success;
        public Order? EditItem { get; set; }

        public OrderEdit()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            using (var context = Config.GetDbContext())
            {
                if (Id != null)
                {
                    EditItem = context.Orders.Find(Id);
                }
                else
                {
                    EditItem = new Order
                    {
                      
                    };
                }
            }
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            using (var context = Config.GetDbContext())
            {
                //if (string.IsNullOrWhiteSpace())
                //{
                //    Success?.Invoke(false, "Client Table ID cannot be empty.");
                //    return;
                //}

                if (Id != null && Id > 0)
                {
                    // Редагування замовлення
                  

                    context.Orders.Update(EditItem);
                }
                else
                {
                    // Додавання нового замовлення
                    var newOrder = new Order
                    {
                      
                    };
                    context.Orders.Add(newOrder);
                }

                context.SaveChanges();
                Success?.Invoke(true, string.Empty);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Success?.Invoke(false, "Operation canceled.");
        }
    }
}
