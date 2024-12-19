﻿
// Критична секція( lock(простий, автоматичний вихід), Monitor(більший контроль над кодом), Mutex) -
// це частина коду, яка доступна лише одному потоку в будь-який момент часу. 
// Вона використовується для захисту спільних ресурсів (змінних, об'єктів, файлів) 
// від одночасного доступу кількох потоків, що може призвести до непередбачуваних результатів.

// Mutex - це механізм синхронізації, який отримується від ОС (живе над процесами). 
// Використовується для забезпечення взаємного виключення доступу до спільних ресурсів. 
// Іменований м'ютекс дозволяє синхронізувати потоки між різними процесами.
// Mutexes є об’єктами ядра (в той час як критичні секції належать процесу), тому їх можна використовувати для синхронізації потоків із різних процесів.
// Наприклад, два процеси, які розділяють спільну пам’ять через відображення файлів, можуть використовувати м’ютекс для синхронізації доступу до неї.
// Так як м’ютекси є об’єктами ядра і мають складнішу реалізацію, на їх обробку, як правило, витрачається більше часу порівняно з критичними секціями.
// Тому, якщо потрібно синхронізувати роботу потоків одного процесу, варто користуватися критичними секціями, в інших випадках - ним.


// Classwork

//if (args.Length > 0)
//{
//    Console.WriteLine($"Mutex demo {args[0]}");
//}
//else
//{
//    Console.WriteLine("Mutex demo no args");
//}
//var m = new Mutex(false);
//Mutex mutex = new Mutex(false, "54A8B9A3-9C9A-4183-9800-A3B90015F94C", out bool isNew);
//for (int i = 0; i < 100; i++)
//{
//    if (mutex.WaitOne(TimeSpan.FromSeconds(10)))
//    {
//        Console.WriteLine($"Counter {i} time {DateTime.Now}");
//        Thread.Sleep(TimeSpan.FromSeconds(1));
//        mutex.ReleaseMutex();
//    }
//    else
//    {
//        Console.WriteLine("Error enter mutex");

//    }
//}





// Practice

// одночасно 3 програми з таймером 1сек
//  arg[0] - current time
// запис у спільний файл c:/temp/mutex.log


string logFilePath = @"C:\Windows\Temp\mutex.log.txt";
string message = string.Empty;

if (args.Length > 0)
{
    message = $"Mutex demo {args[0]}";
}
else
{
    message = "Mutex demo no args";
}
File.AppendAllText(logFilePath, message);


Mutex mutex = new Mutex(false, "54A8B9A3-9C9A-4183-9800-A3B90015F94C", out bool isNew);

for (int i = 0; i < 100; i++)
{
    if (mutex.WaitOne(TimeSpan.FromSeconds(10)))
    {
        string logMessage = $"Counter {i} time {DateTime.Now}{Environment.NewLine}";
        File.AppendAllText(logFilePath, logMessage);
        Console.WriteLine($"Counter {i} time {DateTime.Now}{Environment.NewLine}");
        Thread.Sleep(TimeSpan.FromSeconds(1));
        mutex.ReleaseMutex();
    }
    else
    {
        string logMessage = $"Error time {DateTime.Now}{Environment.NewLine}";
        File.AppendAllText(logFilePath, logMessage);
        Console.WriteLine($"Error time {DateTime.Now}{Environment.NewLine}");
        Thread.Sleep(TimeSpan.FromSeconds(1));
    }
}

