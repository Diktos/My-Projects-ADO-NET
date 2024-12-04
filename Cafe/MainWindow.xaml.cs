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
using Azure;
using Cafe.Data;
using Cafe.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginFrame.LoginResult += LoginFrame_LoginResult;
        }

        private void LoginFrame_LoginResult(int? arg1, string arg2)
        {
            if (arg1 == null)
            {
                Close();
                return;
            }
            Config.UserId = arg1.Value;
            Config.UserName = arg2;
            Title = $"{Config.UserName} : {DateTime.Now}";
            LoginFrame.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Visible;
        }

        void CloseFrames()
        {
            OrderItemsFrame.Visibility = Visibility.Collapsed;
           OrderEditFrame.Visibility = Visibility.Collapsed;
            NomenclatureEditFrame.Visibility = Visibility.Collapsed;
            NomenclatureItemsFrame.Visibility = Visibility.Collapsed;
            MainMenu.Visibility=Visibility.Collapsed;
        }


        private void NomenclatureItemsFrame_Click(object sender, RoutedEventArgs e) // Входим у вікно для роботи з номенклатурами
        {
            NomenclatureItemsFrame.Refresh();
            CloseFrames();
            NomenclatureItemsFrame.Visibility = Visibility.Visible;

            NomenclatureItemsFrame.EditItem += NomenclatureItemsFrame_EditItem; // Подія для зміни об'єкта
            NomenclatureEditFrame.Success += NomenclatureEditFrame_Success; // Залежність подальших дій від результату (0,1)
            NomenclatureItemsFrame.RemoveItem += NomenclatureItemsFrame_RemoveItem;
            NomenclatureItemsFrame.CloseWindow += NomenclatureItemsFrame_CloseWindow;
        }

        private void NomenclatureItemsFrame_CloseWindow()
        {
            CloseFrames();
            MainMenu.Visibility=Visibility.Visible;
        }

        private async void NomenclatureItemsFrame_RemoveItem(int? obj) // Фінальні штрихи у видаленні айтема
        {
            if (obj.HasValue)
            {
                // Показуємо повідомлення для підтвердження
                ActionСonfirmationFrame.InformationText.Text = "Are you sure you want to remove this object?";

                // await дозволяє зупинити виконання коду на цьому рядку, не блокуючи потік, і очікуючи виконання завданння, яке нам шось дасть як результат
                bool isSuccess = await ActionСonfirmationFrame.WaitForUserResponseAsync();

                if (isSuccess)
                {
                    // Видаляємо об'єкт
                    NomenclatureItemsFrame.RemoveItemById(obj.Value);
                    NomenclatureItemsFrame.SelectedNomenclature = null;
                    NomenclatureItemsFrame.Refresh();
                }
                else
                {
                    // Користувач скасував операцію
                    NomenclatureEditFrame_Success(false, "Operation canceled!");
                }
            }
            else
            {
                // Якщо об'єкт не вибрано, показуємо помилку
                ErrorExceptionFrame.ErrorText.Text = "First, select the object to remove!";
                ErrorExceptionFrame.Visibility = Visibility.Visible;
            }
        }
         
        private void NomenclatureEditFrame_Success(bool arg1, string? arg2) // Отримуєм результат спрацювання контрола для зміни і фінальні вікна робим
        {
            if (arg1)
            {
                CloseFrames();
                NomenclatureItemsFrame.Refresh();
                NomenclatureItemsFrame.Visibility = Visibility.Visible;
            }
            else
            {
                CloseFrames();
                ErrorExceptionFrame.ErrorText.Text = arg2;

                NomenclatureItemsFrame.Visibility = Visibility.Visible;
                ErrorExceptionFrame.Visibility = Visibility.Visible;
            }
        }

        private void NomenclatureItemsFrame_EditItem(int? obj) // Передаємо у контрол для зміни інфу про тотем
        {
            CloseFrames();
            if (obj == null)
            {
                NomenclatureEditFrame.Id=0;
            }

            else if (obj.Value > 0)
            {
                NomenclatureEditFrame.Id=obj.Value;
            }

            NomenclatureEditFrame.Visibility = Visibility.Visible;
        }



































        //        --------> TO DO     --------> TO DO  --------> TO DO  --------> TO DO  --------> TO DO  --------> TO DO --------> TO DO





        private void ButtonOrders_Click(object sender, RoutedEventArgs e) // Вхід у вікно роботи із замовленнями
        {
            OrderItemsFrame.Refresh();
            CloseFrames();
            OrderItemsFrame.Visibility = Visibility.Visible;

            OrderItemsFrame.EditOrder += OrderItemsFrame_EditOrder; // Подія для редагування замовлення
            OrderItemsFrame.Remove_Order += OrderItemsFrame_Remove_Order; // Подія для видалення замовлення
            OrderItemsFrame.CloseWindow += OrderItemsFrame_CloseWindow; // Подія для повернення до головного меню
            OrderEditFrame.Success += OrderEditFrame_Success; // Результат редагування/додавання
        }


        private void OrderItemsFrame_CloseWindow()
        {
            CloseFrames();
            MainMenu.Visibility = Visibility.Visible;
        }

        private async void OrderItemsFrame_Remove_Order(int? obj) // Видалення замовлення  --------> TO DO
        {
            if (obj.HasValue)
            {
                // Підтвердження дії
                ActionСonfirmationFrame.InformationText.Text = "Are you sure you want to remove this order?";
                bool isSuccess = await ActionСonfirmationFrame.WaitForUserResponseAsync();

                if (isSuccess)
                {
                    // Видаляємо замовлення
                    OrderItemsFrame.RemoveOrderById(obj.Value);
                    OrderItemsFrame.SelectedOrder = null;
                    OrderItemsFrame.Refresh();
                }
                else
                {
                    // Скасування операції
                    OrderEditFrame_Success(false, "Operation canceled!");
                }
            }
            else
            {
                // Oб'єкт не вибрано
                ErrorExceptionFrame.ErrorText.Text = "First, select an order to remove!";
                ErrorExceptionFrame.Visibility = Visibility.Visible;
            }
        }

        private void OrderEditFrame_Success(bool arg1, string? arg2) // Результат редагування/додавання
        {
            if (arg1)
            {
                CloseFrames();
                OrderItemsFrame.Refresh();
                OrderItemsFrame.Visibility = Visibility.Visible;
            }
            else
            {
                CloseFrames();
                ErrorExceptionFrame.ErrorText.Text = arg2;

                OrderItemsFrame.Visibility = Visibility.Visible;
                ErrorExceptionFrame.Visibility = Visibility.Visible;
            }
        }

        private void OrderItemsFrame_EditOrder(int? obj) // Редагування замовлення --------> TO DO
        {
            CloseFrames();
            //if (obj == null)
            //{
            //    OrderEditFrame.Id = 0; // Нове замовлення
            //}
            //else if (obj.Value > 0)
            //{
            //    OrderEditFrame.Id = obj.Value; // Редагування існуючого замовлення
            //}

            OrderEditFrame.Visibility = Visibility.Visible;
        }
    }
}
