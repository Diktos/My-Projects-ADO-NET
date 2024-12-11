using MatrixLib;
using System.Text;


//Використовуючи бібліотеку Matrix написати множення матриць з заміром часу виконання

//  в одному потоці
//  в потокох по оному на результатуючу ячейку
//  в потоках по одному на рядок матриці
//  в двох потоках
//  в чотирьох потоках
//  в восьми потоках
//  в 16-ти потоках
//  Зробити тестування для матриці 10х10, 100х100, 1000х1000, 10000х10000.

//  Надати висновки по розрахункам.



Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;


double CalcTimeOnGenerating(Action action)
{
    DateTime start_time = DateTime.Now;

    action();

    return (DateTime.Now - start_time).TotalMilliseconds;
}

var matrixLib = new Matrix(); 

int[] threadCounts = { 1, 2, 4, 8, 16 }; // Для кращої читабельності - кількість потоків для тестування для завдання
int[] dimensions = { 10, 100, 1000, 10000 }; // Квадратичні габарити матриці
//List<double> executionTimeList = new List<double>();
int dimension = dimensions[0]; // Квадратичні габарити матриці

var a = matrixLib.CreateMatrix(dimension);
var b = matrixLib.CreateMatrix(dimension);
var c = new double[dimension, dimension];

// 1 Тест

void MultiplyElements()
{
    matrixLib.MultiplySingleThread(a, b, c, dimension);
}

Console.WriteLine($"Час виконання обчислень в 1 потоці: {CalcTimeOnGenerating(() => MultiplyElements())} мілісекунд");


// 2 Тест

// () => Лямбда-вираз, який створює анонімний метод, що відповідає типу Action
Console.WriteLine($"Час виконання обчислень в потокох по одному на результатуючу ячейку: {CalcTimeOnGenerating(() => matrixLib.MultiplyEachElementInThread(a, b, c, dimension))} мілісекунд");

//3 Тест

Console.WriteLine($"Час виконання обчислень в потоках по одному на рядок матриці: {CalcTimeOnGenerating(() => matrixLib.MultiplyEachRowInThread(a, b, c, dimension))} мілісекунд");


//4 Тест

Console.WriteLine($"Час виконання обчислень в 2 потоках: {CalcTimeOnGenerating(() => matrixLib.matrixMultiplyDynamicThreads(a, b, c, dimension,2))} мілісекунд");


//5 Тест

Console.WriteLine($"Час виконання обчислень в 4 потоках: {CalcTimeOnGenerating(() => matrixLib.matrixMultiplyDynamicThreads(a, b, c, dimension, 4))} мілісекунд");


//6 Тест

Console.WriteLine($"Час виконання обчислень в 8 потоках: {CalcTimeOnGenerating(() => matrixLib.matrixMultiplyDynamicThreads(a, b, c, dimension, 8))} мілісекунд");


//7 Тест

Console.WriteLine($"Час виконання обчислень в 16 потоках: {CalcTimeOnGenerating(() => matrixLib.matrixMultiplyDynamicThreads(a, b, c, dimension, 16))} мілісекунд");



// Висновок: 

// Обрахунок залежить віл потужності процесора (кількості ядер) і нагрузки на нього в поточний момент часу.
// Для процесора з 4 ядрами, найбільш ефективним є використання від 2 до 4 потоків для обчислень.
// Легковажне збільшення кількості потоків більше за кількість ядер може призвести до погіршення продуктивності через зайві витрати на управління потоками.