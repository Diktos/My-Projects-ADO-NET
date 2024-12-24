using System.Threading;

namespace MatrixLib
{
    // При замірах краще [], ніж List, бо ця колекція з динамічним розміром,
    // яка автоматично розширюється, коли в неї додаються нові елементи.
    // Це передбачає копіювання елементів у новий більший масив, якщо початковий розмір перевищено.
    // Наслідки:
    // Накладні витрати на копіювання даних, особливо при великій кількості завдань.
    // Додатковий час на управління пам'яттю.
    // Чому це не проблема для []:
    // Масив [] має фіксований розмір, який задається одразу,
    // і не потребує динамічного розширення, що усуває ці накладні витрати.
    public class Matrix
    {
        #region
        public static double[,] CreateMatrix(int dimension)
        {
            var result = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    result[i, j] = Random.Shared.NextDouble();
                }
            }
            return result;
        }

        public static void MultiplreOneElement(object? param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));
            MatrixParams matrixParams = (MatrixParams)param;
            double result = 0;
            for (int mi = 0; mi < matrixParams.dim; mi++)
            {
                result = result + matrixParams.a[matrixParams.i, mi] * matrixParams.b[mi, matrixParams.j];
            }
            matrixParams.c[matrixParams.i, matrixParams.j] = result;
        }

        #endregion


        // Створення методів класу з використанням потоків

        #region
        // Множення в одному потоці

        public void MultiplySingleThread(double[,] a, double[,] b, double[,] c, int dimension, Action<int>? progress)
        {
            Thread thread = new Thread(() =>
            {
                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        // Заповнюємо параметри для результатуючої матриці
                        var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                        MultiplreOneElement(matrixParams);
                    }
                    progress?.Invoke(i);
                }
            });
            thread.Start();
        }


        // Кожна комірка множиться в окремому потоці 
        public void MultiplyEachElementInThread(double[,] a, double[,] b, double[,] c, int dimension, Action<int>? progress)
        {
            var threads = new Thread[dimension * dimension];
            var ind = 0;
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    // Заповнюємо параметри для результатуючої матриці 
                    var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                    threads[ind] = new Thread(MultiplreOneElement);
                    threads[ind++].Start(matrixParams);
                }
                progress?.Invoke(i); // 
            }
        }


        // Кожен рядок множиться в окремому потоці (1/dim)
        public void MultiplyEachRowInThread(double[,] a, double[,] b, double[,] c, int dimension, Action<int>? progress)
        {
            var threads = new Thread[dimension];
            int elementsProcessed = 0;
            for (int i = 0; i < dimension; i++)
            {
                var row = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        var matrixParams = new MatrixParams(dimension, row, j, a, b, c, null);
                        MultiplreOneElement(matrixParams);
                    }
                });
                threads[i].Start();
                elementsProcessed++;
                progress?.Invoke(elementsProcessed);
            }
        }


        // Множення в 2 потоках (без гнучкості у виборі кількості потоків)
        public void MultiplyInTwoThreads(double[,] a, double[,] b, double[,] c, int dimension)
        {
            // Визначаємо анонімний метод для роботи потоком № 1: 
            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < dimension / 2; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                        MultiplreOneElement(matrixParams);
                    }
                }
            });

            // Визначаємо анонімний метод для роботи потоком № 2: 
            Thread thread2 = new Thread(() =>
            {
                for (int i = dimension / 2; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                        MultiplreOneElement(matrixParams);
                    }
                }
            });
            thread1.Start();
            thread2.Start();
        }


        // Переваги: Динамічний вибір кількості потоків для множення матриць
        public void matrixMultiplyDynamicThreads(double[,] a, double[,] b, double[,] c, int dimension, int threadCount, Action<int>? progress) 
        {
            int rowsPerThread = dimension / threadCount; // Змінна для визначення кількості роботи для одного потока 40/4=10
            Thread[] threads = new Thread[threadCount];
            int elementsProcessed = 0;

            for (int thrInd = 0; thrInd < threadCount; thrInd++)
            {
                int startRow = thrInd * rowsPerThread; // Визначення координції початку у матриці // 10*2=20
                int endRow = (thrInd == threadCount - 1) ? dimension : startRow + rowsPerThread; // Визначення координції грані матриці
                // 2!=4-3 => 20+10 = 30

                // Отримавши всі необхідні вхідні дані - обчислюємо
                threads[thrInd] = new Thread(() =>
                {
                    for (int i = startRow; i < endRow; i++)
                    {
                        for (int j = 0; j < dimension; j++)
                        {
                            var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                            MultiplreOneElement(matrixParams);
                        }
                        elementsProcessed++;
                        progress?.Invoke(elementsProcessed+1);
                    }      
                });
                threads[thrInd].Start();
            }
        }
        #endregion



        // Створення методів класу з використанням тасків
        #region

        // Множення в одному завданні
        public void MultiplySingleTask(double[,] a, double[,] b, double[,] c, int dimension, Action<int>? progress)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                        MultiplreOneElement(matrixParams); 
                    }
                    progress?.Invoke(i);
                    // Тут оновлення було знизу, щоб уникнути значно більшу кількість викликів progress?.Invoke.
                    // Це може створити додаткове навантаження на Dispatcher, коли великі матриці
                    // потенційно уповільнити роботу через блокування при оновленні UI,якщо після кожної ітерації оновлювати.

                }
            });
        }

        // Кожен елемент множиться окремим завданням
        public void MultiplyEachElementInTask(double[,] a, double[,] b, double[,] c, int dimension, Action<int>? progress)
        {
            Task[] tasks = new Task[dimension*dimension];
            int taskIndex = 0;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                    tasks[taskIndex++] = Task.Run(() =>
                    { 
                        MultiplreOneElement(matrixParams);
                        progress?.Invoke(taskIndex);
                     });
                } 
               
            }
        }


        // Кожен рядок множиться окремим завданням
        public void MultiplyEachRowInTask(double[,] a, double[,] b, double[,] c, int dimension, Action<int>? progress)
        {
            Task[] tasks = new Task[dimension];
            int elementsProcessed = 0;
            for (int i = 0; i < dimension; i++)
            {
                int row = i; // Локальна змінна для уникнення замикання
                tasks[row] = Task.Run(() =>
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        var matrixParams = new MatrixParams(dimension, row, j, a, b, c, null);
                        MultiplreOneElement(matrixParams);
                    }
                    elementsProcessed++;
                    progress?.Invoke(elementsProcessed);
                }); 
             
            }
        }


        // Динамічний вибір кількості завдань
        public void MatrixMultiplyDynamicTasks(double[,] a, double[,] b, double[,] c, int dimension, int taskCount, Action<int>? progress)
        {
            int rowsPerTask = dimension / taskCount;
            Task[] tasks = new Task[taskCount];
            int progressBarNum = 0;
            for (int taskIndex = 0; taskIndex < taskCount; taskIndex++)
            {
                int startRow = taskIndex * rowsPerTask;
                int endRow = (taskIndex == taskCount - 1) ? dimension : startRow + rowsPerTask;

                tasks[taskIndex] = Task.Run(() =>
                {
                    for (int i = startRow; i < endRow; i++)
                    {
                        for (int j = 0; j < dimension; j++)
                        {
                            var matrixParams = new MatrixParams(dimension, i, j, a, b, c, null);
                            MultiplreOneElement(matrixParams);
                        }
                        progressBarNum++;
                        progress?.Invoke(progressBarNum);
                    }
                });
            }
        }
        #endregion
    }
}
