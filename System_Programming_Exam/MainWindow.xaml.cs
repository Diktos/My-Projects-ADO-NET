using System;
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
using MatrixLib;

namespace System_Programming_Exam
{
    public partial class MainWindow : Window
    {
        public MatrixLib.Matrix _MatrixLib {  get; set; }  
        public double[,] MatrixA { get; set; }
        public double[,] MatrixB { get; set; }
        public double[,] ResultMatrix { get; set; }
        public int Dimension { get; set; }
        public ThreadPriority _ThreadPriority { get; set; } = ThreadPriority.Lowest;
        public bool IsStopped { get; set; }
        private CancellationTokenSource cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _MatrixLib = new MatrixLib.Matrix();
            MatrixA = MatrixLib.Matrix.CreateMatrix(Dimension);
            MatrixB = MatrixLib.Matrix.CreateMatrix(Dimension);
            ResultMatrix = new double[Dimension, Dimension];
        }

        public void UpdateProgressBar(ProgressBar progressBar, int progress, int dimension)
        {
            Dispatcher.Invoke(() =>
            {
                progressBar.Value = (((double)progress + 1) / dimension * 100);
            });
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            // Щоб не можна було активовувати кнопку до виконання роботи
            StartTestButton.IsEnabled = false; 
            StopTestButton.IsEnabled = true;
            IsStopped = false;

            // Initialize cancellation token source
            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Для того, щоб користувач не чудив - присвоюєм значення яке він вів, до того як нажав кнопку (не міг під час роботи змінити нічо)
            int dimension = Dimension;

            MatrixA = MatrixLib.Matrix.CreateMatrix(dimension);
            MatrixB = MatrixLib.Matrix.CreateMatrix(dimension);
            ResultMatrix = new double[dimension, dimension];

            SetThreadPriority();

            if (ThreadCheckBox1.IsChecked == true)
            {
                if (ThreadProgressBar1.Value <= 0)
                {
                    ThreadProgressBar1.Value = 0;
                }
                _MatrixLib.MultiplySingleThread(MatrixA, MatrixB, ResultMatrix, dimension, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    UpdateProgressBar(ThreadProgressBar1, progress, dimension);
                });
            }

            if (ThreadCheckBox2.IsChecked == true)
            {
                _MatrixLib.MultiplyEachRowInThread(MatrixA, MatrixB, ResultMatrix, dimension, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    UpdateProgressBar(ThreadProgressBar2, progress, dimension);
                });
            }

            if (ThreadCheckBox3.IsChecked == true)
            {
                _MatrixLib.MultiplyEachElementInThread(MatrixA, MatrixB, ResultMatrix, dimension, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    UpdateProgressBar(ThreadProgressBar3, progress, dimension);
                });
            }

            if (ThreadCheckBox4.IsChecked == true)
            {
                int threadCount=0;
                if (!int.TryParse(NumberOfThreads.Text.ToString(), out threadCount))
                {
                     threadCount = 1;
                }
                if (threadCount <= 0)
                {
                    threadCount = 1;
                }
                _MatrixLib.matrixMultiplyDynamicThreads(MatrixA, MatrixB, ResultMatrix, dimension, threadCount, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    Dispatcher.Invoke(() =>
                    {
                        ThreadProgressBar4.Value = ((double)progress + 1) / dimension * 100;
                    });
                });
            }

            if (TaskCheckBox1.IsChecked == true)
            {
                _MatrixLib.MultiplySingleTask(MatrixA, MatrixB, ResultMatrix, dimension, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    UpdateProgressBar(TaskProgressBar1, progress, dimension);
                });
            }

            if (TaskCheckBox2.IsChecked == true)
            {
                _MatrixLib.MultiplyEachRowInTask(MatrixA, MatrixB, ResultMatrix, dimension, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    UpdateProgressBar(TaskProgressBar2, progress, dimension);
                });
            }

            if (TaskCheckBox3.IsChecked == true)
            {
                _MatrixLib.MultiplyEachElementInTask(MatrixA, MatrixB, ResultMatrix, dimension, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    UpdateProgressBar(TaskProgressBar3, progress, dimension);
                });
            }

            if (TaskCheckBox4.IsChecked == true)
            {
                int taskCount = 0;
                if (!int.TryParse(NumberOfTasks.Text.ToString(), out taskCount))
                {
                     taskCount = 1; 
                }
                if (taskCount <= 0)
                {
                    taskCount = 1;
                }
                _MatrixLib.MatrixMultiplyDynamicTasks(MatrixA, MatrixB, ResultMatrix, dimension, taskCount, progress =>
                {
                    if (IsStopped || cancellationToken.IsCancellationRequested) return;
                    Dispatcher.Invoke(() =>
                    {
                        TaskProgressBar4.Value = ((double)progress / dimension) * 100;
                    });
                });
            }
        }

        private void SetThreadPriority()
        {
            switch (ThreadPriorityComboBox.SelectedIndex)
            {
                case 0:
                    _ThreadPriority = ThreadPriority.Lowest;
                    break;
                case 1:
                    _ThreadPriority = ThreadPriority.BelowNormal;
                    break;
                case 2:
                    _ThreadPriority = ThreadPriority.Normal;
                    break;
                case 3:
                    _ThreadPriority = ThreadPriority.AboveNormal;
                    break;
                case 4:
                    _ThreadPriority = ThreadPriority.Highest;
                    break;
            }
            Thread.CurrentThread.Priority=_ThreadPriority;
        }

        private void StopTest_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource?.Cancel();

            IsStopped = true;
            StartTestButton.IsEnabled = true;
            StopTestButton.IsEnabled = false;

            // Скидання value прогрес-барів
            ThreadProgressBar1.Value = 0;
            ThreadProgressBar2.Value = 0;
            ThreadProgressBar3.Value = 0;
            ThreadProgressBar4.Value = 0;
            TaskProgressBar1.Value = 0;
            TaskProgressBar2.Value = 0;
            TaskProgressBar3.Value = 0;
            TaskProgressBar4.Value = 0;
        }
    }
}