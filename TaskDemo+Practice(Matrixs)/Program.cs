using MatrixLib;
using System.Text;




// Task - > Працює на фоні(повертає результат, на відміну від thread). Може змінювати поведінку за необхідності.
// За допомогою await(повідомляємо операційно систему відразу, що він в черзі(не чіпати його поки він не закінчить)) -
// вказуємо, що операція асинхронна, щоб на неї не виділявся процесорний час (дьоргали потік(питали))

// Синхронна - не потрібно чекати(виконується наскільки швидко(одночасно, немає затримок)), наприклад,
// Доступ до оперативної пам’яті виконується дуже швидко.
// Результат доступний одразу, без затримок, тому немає сенсу робити його асинхронним.

// Асинхронна операція - потребує чекання (працює у фоновому режимі), наприклад,
// Завантаження даних із сервера чи зчитування великого файлу,
// залежать від зовнішніх ресурсів (сервер, диск, швидкість Інтернету).

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;



var matrixLib = new Matrix();

int[] threadCounts = { 1, 2, 4, 8, 16 }; // Для кращої читабельності - кількість потоків для тестування для завдання
int[] dimensions = { 10, 100, 1000, 10000 }; 

int dimension = dimensions[0]; // Квадратичні габарити матриці
var a = Matrix.CreateMatrix(dimension);
var b = Matrix.CreateMatrix(dimension);
var c = new double[dimension, dimension];

double CalcTimeOnGenerating(Action action)
{
    DateTime start_time = DateTime.Now;

    action();

    return (DateTime.Now - start_time).TotalSeconds;
}
double CalcTimeOnGeneratingOnMilliseconds(Action action)
{
    DateTime start_time = DateTime.Now;

    action();

    return (DateTime.Now - start_time).TotalMilliseconds;
}



// Практичне завдання з використанням завдань(Tasks)

for (int i = 0; i < dimensions.Length; i++)
{
    dimension = dimensions[i]; // Квадратичні габарити матриці

    a = Matrix.CreateMatrix(dimension);
    b = Matrix.CreateMatrix(dimension);
    c = new double[dimension, dimension];

    Console.WriteLine($"Матриця на {dimension}: ");

    // 1 Тест 
    Console.WriteLine($"Час виконання обчислень в 1 завданні: {CalcTimeOnGeneratingOnMilliseconds(() => matrixLib.MultiplySingleTask(a, b, c, dimension))} мілісекунд");

    // 2 Тест 
    //Console.WriteLine($"Час виконання обчислень в завданнях по одному на результатуючу ячейку: {CalcTimeOnGeneratingOnMilliseconds(() => matrixLib.MultiplyEachElementInTask(a, b, c, dimension))} мілісекунд");

    // 3 Тест 
    Console.WriteLine($"Час виконання обчислень в завданнях по одному на рядок матриці: {CalcTimeOnGeneratingOnMilliseconds(() => matrixLib.MultiplyEachRowInTask(a, b, c, dimension))} мілісекунд");

    // 4 Тест 
    Console.WriteLine($"Час виконання обчислень в 2 завданнях: {CalcTimeOnGeneratingOnMilliseconds(() => matrixLib.MatrixMultiplyDynamicTasks(a, b, c, dimension, 2))} мілісекунд");

    // 5 Тест 
    Console.WriteLine($"Час виконання обчислень в 4 завданнях: {CalcTimeOnGeneratingOnMilliseconds(() => matrixLib.MatrixMultiplyDynamicTasks(a, b, c, dimension, 4))} мілісекунд");

    // 6 Тест 
    Console.WriteLine($"Час виконання обчислень в 8 завданнях: {CalcTimeOnGeneratingOnMilliseconds(() => matrixLib.MatrixMultiplyDynamicTasks(a, b, c, dimension, 8))} мілісекунд");

    // 7 Тест 
    Console.WriteLine($"Час виконання обчислень в 16 завданнях: {CalcTimeOnGeneratingOnMilliseconds(() => matrixLib.MatrixMultiplyDynamicTasks(a, b, c, dimension, 16))} мілісекунд");

    Console.WriteLine(string.Empty);
}

// Завдання (Task) у багатьох випадках показують кращу продуктивність через використання
// Thread Pool(повторно використовуються існуючі потоки (менше перемикань,виділення пам’яті,створень, завершень, що також не безкоштовно)). 
// Зі збільшенням розміру матриці та кількості завдань Task краще масштабуються, оскільки .NET управляє оптимальним використанням системних ресурсів через пул потоків,
// а threads потребують більше ручного керування, що ускладнює їх використання.
// Task краще використовувати, наприклад,
// Коли потрібно виконати асинхронну операцію, наприклад: читання/запис файлів, запити до бази даних, HTTP-запити.
// Thread краще використовувати, наприклад,
// Коли потрібно контролювати виконання окремих потоків, і завдання повинні працювати паралельно, без асинхронного підходу.










// Classwork

//void CallBackFunction(object state)
//{
//    Console.WriteLine("CallBackFunction  executed");

//}

//void ImitationAsync(Action<object> callback)
//{
//    Thread.Sleep(1000);
//    callback("Imitation end");
//}

//await Task.Run(() => ImitationAsync(CallBackFunction)); //read disk

//var a = Console.ReadLine();
//Console.WriteLine(a);


//Thread.Sleep(1000);

//  -> Відправляємо контролеру запит на сектор (Трек,номер сектора)
//  <- підтвердження від контролера про розуміння операції
//  ********
// loop (pause(),   check controller state) // nop

//  -> Відправляємо контролеру запит на сектор (Трек,номер сектора,номер прериваня (call back))
//  <- підтвердження від контролера про розуміння операції
//  процесор продовжує виконувати програму
//  .......
// преривання (виконуэться зчитування з контролера даних)