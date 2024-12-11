using System.Threading;

namespace MatrixLib
{
    public class Matrix
    {
        #region
        public double[,] CreateMatrix(int dimension)
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

        public void MultiplreOneElement(object? param) // Один вихідний елемент при множенні
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


        // Множення в одному потоці
        public void MultiplySingleThread(double[,] a, double[,] b, double[,] c, int dimension)
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
                }
            });
            thread.Start();
        }


        // Кожна комірка множиться в окремому потоці 
        public void MultiplyEachElementInThread(double[,] a, double[,] b, double[,] c, int dimension)
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
                    threads[ind].Start(matrixParams);
                    ind++;
                }
            }
        }


        // Кожен рядок множиться в окремому потоці (1/dim)
        public void MultiplyEachRowInThread(double[,] a, double[,] b, double[,] c, int dimension)
        {
            var threads = new Thread[dimension];
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
        public void matrixMultiplyDynamicThreads(double[,] a, double[,] b, double[,] c, int dimension, int threadCount) 
        {
            int rowsPerThread = dimension / threadCount; // Змінна для визначення кількості роботи для одного потока 40/4=10
            Thread[] threads = new Thread[threadCount]; 

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
                    }
                });
                threads[thrInd].Start();
            }
        }
        
    }
}
