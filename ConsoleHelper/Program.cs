using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public static class ConsoleHelper
{

    //public static void WriteServerMessage(string message)
    //{
    //    if (message.Contains("Connection"))
    //    {
    //        Console.ForegroundColor = ConsoleColor.Green;
    //    }
    //    else if (message.Contains("Exit"))
    //    {
    //        Console.ForegroundColor = ConsoleColor.Red;
    //    }
    //    else
    //    {
    //        Console.ForegroundColor = ConsoleColor.Yellow;
    //    }

    //    Console.WriteLine(message);
    //    Console.ResetColor();
    //}


    public static void WriteConnectionMessage(string clientName, ConcurrentDictionary<string, TcpClient> clients)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        string formattedMessage = $"[Server]: {clientName} connection";
        Console.WriteLine(formattedMessage);

        // Кажемо про те, що зайшов новий клієнт, окрім тому - хто зайшов
        foreach (var kvp in clients)
        {
            if (kvp.Key != clientName)
            {
                var clientStream = kvp.Value.GetStream();
                byte[] responseData = Encoding.UTF8.GetBytes(formattedMessage);
                clientStream.Write(responseData, 0, responseData.Length);
                Console.ResetColor();
            }
        }
    }


    public static void WriteExitMessage(string clientName, ConcurrentDictionary<string, TcpClient> clients)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        string formattedMessage = $"[Server]: {clientName} exit!";
        Console.WriteLine(formattedMessage);

        // Кажемо про те, що вийшов клієнт, окрім тому - хто вийшов
        foreach (var kvp in clients)
        {
            if (kvp.Key != clientName)
            {
                var clientStream = kvp.Value.GetStream();
                byte[] responseData = Encoding.UTF8.GetBytes(formattedMessage);
                clientStream.Write(responseData, 0, responseData.Length);
                Console.ResetColor();
            }
        }
    }
}