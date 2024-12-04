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
    public partial class ActionСonfirmationUserControl : UserControl
    {
        private TaskCompletionSource<bool> _tcs;

        //TaskCompletionSource - Чекає подій, які неможливо обернути в Task стандартним способом.
        //

        public ActionСonfirmationUserControl()
        {
            InitializeComponent();
        }

        public Task<bool> WaitForUserResponseAsync() // Асинхронне очікування відповіді користувача
        {
            _tcs = new TaskCompletionSource<bool>();

            // Показуємо контрол
            this.Visibility = Visibility.Visible;

            // Повертаємо Task, який завершиться після натискання кнопок
            return _tcs.Task;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            _tcs?.SetResult(true); // Завершуємо Task з результатом true
            this.Visibility = Visibility.Collapsed;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _tcs?.SetResult(false); // Завершуємо Task з результатом false
            this.Visibility = Visibility.Collapsed;
        }
    }

}
