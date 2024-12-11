using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFThreadDemo
{
    public partial class MainWindow : Window
    {
        private System.Timers.Timer myTimer;

        private DispatcherTimer myDispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            // Для myDispatcherTimer - краще, коли у нас э UI (є в потоці), не варто використовувати на фоні - може фрізити
            myDispatcherTimer = new DispatcherTimer();
            myDispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            myDispatcherTimer.Tick += MyDispatcherTimer_Tick; 
            myDispatcherTimer.Start();

            // Для myTimer
            myTimer = new System.Timers.Timer(); // myTimer - краще, коли у нас нема UI(потребує окремого потоку), краще, коли потрібна робота на фоні
            myTimer.Interval = 1000;
            myTimer.Elapsed += MyTimer_Elapsed; // Elapsed(минуло) - виникає кожного разу, коли таймер досягає встановленого інтервалу часу
            myTimer.Start();
        }

        private void MyTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TimerClock.Text = DateTime.Now.ToString("HH:MM:ss");
            });
        }

        private void MyDispatcherTimer_Tick(object? sender, EventArgs e)
        {
            DispatcherClock.Text = DateTime.Now.ToString("HH:MM:ss");
        }

    }
}