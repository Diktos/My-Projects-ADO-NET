using System.Net.Sockets;
using System.Net;
using System.Text;

var ipaddress = IPAddress.Parse("127.0.0.1"); // Айпі адрес машини
IPEndPoint iPEndPoint = new IPEndPoint(ipaddress, 10000);
var socket = new Socket(ipaddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(iPEndPoint);
bool flag=false;
if (socket.Connected)
{
    Console.WriteLine("Connected to server. You can start typing your messages!");

    while (true)
    {
        // Асинхронне приймання повідомлень від сервера
        //Task.Run(() =>
        //{

        //  Повідомлень на сервер
        Console.Write("-->");
        string message = Console.ReadLine() + "\r\n";
        var data = Encoding.UTF8.GetBytes(message);
        socket.Send(data);

        var buffer = new byte[1024];
        var response = new StringBuilder();
        int bytes = 0;


        //while (true)
        //{
        //response.Clear();
        //bytes = 0;
        do
        {
            bytes = socket.Receive(buffer, buffer.Length, 0);
            response.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
        } while (!response.ToString().EndsWith("\r\n") || bytes <= 0);

        Console.WriteLine();
        Console.WriteLine($"Response from server: {response}");
        if (response.ToString().Contains("EOF\r\n"))
        {
            flag = true;
            break;

        }
    }
      
    //}

    //});

    //// Асинхронне відправлення повідомлень на сервер
    //Task.Run(() =>
    //{
    //while (true)
    //{
    //    Console.Write("-->");
    //    string message = Console.ReadLine() + "\r\n";
    //    var data = Encoding.UTF8.GetBytes(message);
    //    socket.Send(data);
    //}
    //});
}
if (flag)
{
    socket.Close();
}
