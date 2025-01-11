using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;




// HomeWork

// Використовуючи як взірець код з лекції, реалізувати сканер зміни титульної сторінки сайту www.google.com.
// Сканер має мати параметер переіодичності сканування (наприклад, 5 секунд).
// Має робити робити підрахунок кількості змін титульної сторінки за первний період або записувати всі зміни в базу.
// Має фіксувати зміни (різницю).*** Ускладнене завдання
// Сканер можна реалізувати як консольний додаток або як (службу Windows *** ускладнене завдання).


// Функція, що повертає код сторінки
string GetPageCode(string hostNameOrAdress, string getRequest)
{
    var iPHostEntry = Dns.GetHostEntry(hostNameOrAdress);
    IPAddress iPAdress = iPHostEntry.AddressList[0];
    IPEndPoint iPEndPoint = new IPEndPoint(iPAdress, 80); // ip + port (80)

    // Створення мережевого з'єднання між додатком і сервером для забезпечення обміну даними між процесами
    var socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    socket.Connect(iPEndPoint);

    var page = new StringBuilder();
    if (socket.Connected)
    {
        var data = Encoding.UTF8.GetBytes(getRequest);

        socket.Send(data, SocketFlags.None);
        var buffer = new byte[1024];
        int bytes = 0;
        do
        {
            bytes = socket.Receive(buffer, buffer.Length, 0);
            page.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
        } while (bytes > 0);
    }
    socket.Close();

    return page.ToString();
}


int checkInterval = 5000;
int changeCount = 0;
// Для збереження попереднього виду сторінки для майбутнього порівняння
string prevResult = GetPageCode("www.google.com", "GET / HTTP/1.1\r\nHost: www.google.com\r\nConnection: close\r\n\r\n");
// Поточний вигляд сторінки в момент сканування
string currentResult = null;
// База, що зберігає зміни (код нової сторінки і час виявлення зміни)
List<Tuple<string, string>> detectedChanges = new List<Tuple<string, string>>();

Console.WriteLine("Scanning started");
do
{
    // Якщо натиснуто ESC - завершення сканування
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        break;
    }

    currentResult = GetPageCode("www.google.com", "GET / HTTP/1.1\r\nHost: www.google.com\r\nConnection: close\r\n\r\n");

    if (currentResult != prevResult)
    {
        Console.WriteLine($"Change detected at {DateTime.Now}");
        changeCount++;
        detectedChanges.Add(Tuple.Create(currentResult, $"Change {changeCount} detected at {DateTime.Now}"));
    }

    prevResult = currentResult;
    Thread.Sleep(checkInterval);

} while (true);
Console.WriteLine($"Total changes detected: {changeCount}");
Console.WriteLine("Scanning stopped");







// Class work
// <div> порахувати кількість слів </div> на www.google.com 

//IPAddress iPAddress = IPAddress.Parse("192.168.1.1");
//iPAddress = new IPAddress(new byte[] { 192, 168, 1, 1 }); // 32 bit
// localhost ID - 127.0.0.1
var google = Dns.GetHostEntry("www.google.com");
IPAddress googleIP = google.AddressList[0];

IPEndPoint iPEndPoint = new IPEndPoint(googleIP, 80); // ip + port

var socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(iPEndPoint);

var page = new StringBuilder();
if (socket.Connected)
{
    string getRequest = "GET / HTTP/1.1\r\nHost: www.google.com\r\nConnection: close\r\n\r\n";
    var data = Encoding.UTF8.GetBytes(getRequest);
    socket.Send(data, SocketFlags.None);

    int bytes = 0;
    var buffer = new byte[1024];
    do
    {
        bytes = socket.Receive(buffer, buffer.Length, 0);
        page.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
    } while (bytes > 0);

    string Page = page.ToString();

    string[] divSections = Page.Split(new[] { "<div"},StringSplitOptions.None);

    Console.WriteLine($"Total word count within <div> tags on Google: {divSections.Length-1}");

    // divSections.Length-1, бо
    // text = "<div><div> <div> Empty, <div> base?"
    // words = text.split("<div>")  Розділення за словом "<div>"
    // ['', '', ' ', 'Empty, ', ' base?'] --> 5 входжень <div>, хоч насправді їх 4
}





// <div> порахувати кількість слів </div> на www.microsoft.com

var microsoft = Dns.GetHostEntry("www.microsoft.com");
IPAddress microsoftIP = microsoft.AddressList[0];

iPEndPoint = new IPEndPoint(microsoftIP, 80); // ip + port

socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(iPEndPoint);
var page1 = new StringBuilder();

if (socket.Connected)
{
    string getRequest = "GET / HTTP/1.1\r\nHost: www.microsoft.com\r\nConnection: close\r\n\r\n";
    var data = Encoding.UTF8.GetBytes(getRequest);
    var buffer = new byte[1024];
    socket.Send(data, SocketFlags.None);
    int bytes = 0;
    do
    {
        bytes = socket.Receive(buffer, buffer.Length, 0);
        page1.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
    } while (bytes > 0);

    string Page1 = page1.ToString();

    string[] divSectionsMicrosoft = Page1.Split(new[] { "<div" }, StringSplitOptions.None);

    Console.WriteLine($"Total word count within <div> tags on Microsoft: {divSectionsMicrosoft.Length-1}");
}

// <div> порахувати кількість слів </div> на www.google.com та www.microsoft.com



