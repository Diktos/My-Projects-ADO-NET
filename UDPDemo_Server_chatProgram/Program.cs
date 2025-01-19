using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.IO;


public class UDPDemo_Server_chatProgram
{
    private static ConcurrentDictionary<IPEndPoint, string> clients = new ConcurrentDictionary<IPEndPoint, string>();
    private static ConcurrentDictionary<string, IPEndPoint> blockedClients = new ConcurrentDictionary<string, IPEndPoint>();
    private static string adminName = "admin";

    public static void Main(string[] args)
    {
        var server = new UDPDemo_Server_chatProgram();
        server.Start();
    }

    public void Start()
    {
        UdpClient udpServer = new UdpClient();
        udpServer.Client.Bind(new IPEndPoint(IPAddress.Any, 10000));
        Console.WriteLine("UDP Chat Server started...");

        Task server = Task.Run(() =>
        {
            Console.WriteLine("Waiting for connection...");
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, 0);
                var data = udpServer.Receive(ref remoteEP);
                var message = Encoding.UTF8.GetString(data);
                HandleMessage(message, remoteEP, udpServer);
            }
        });
        server.Wait();
    }

    private void HandleMessage(string message, IPEndPoint clientEndPoint, UdpClient udpServer)
    {
        var messageParts = message.Split(' ');
        var command = messageParts[0].Trim(); // Trim() - уникаємо у назві команди "\r\n"

        switch (command)
        {
            case "connect":
                if (messageParts.Length == 2)
                {
                    var userName = messageParts[1].Trim(); // Trim() - щоб прибрати зайві \r\n
                    clients[clientEndPoint] = userName; // key(IpEndPoint) = value(string)
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{userName} connected from {clientEndPoint}!");
                    Console.ResetColor();
                    udpServer.Send(Encoding.UTF8.GetBytes("Welcome to our server!"), "Welcome to our server!".Length, clientEndPoint);
                    BroadcastMessage("server", $"{userName} connected!", udpServer);
                }
                break;
            case "send":
                if (messageParts.Length == 3 && clients.Values.Contains(messageParts[1]))
                {
                    var recipient = messageParts[1];
                    var sender = GetClientName(clientEndPoint);
                    var msgContent = string.Join(' ', messageParts, 2, messageParts.Length - 2);
                    SendMessage(sender, recipient, msgContent, udpServer);
                }
                break;
            case "sendall":
                if (messageParts.Length == 2)
                {
                    var sender = GetClientName(clientEndPoint);
                    var msgContent = string.Join(' ', messageParts, 1, messageParts.Length - 1);
                    BroadcastMessage(sender, msgContent, udpServer);
                }
                break;
            case "block":
                if (messageParts.Length == 2 && clients.Values.Contains(messageParts[1].Trim())) // Trim() - уникаємо у перевірці "\r\n"
                {
                    var userToBlock = messageParts[1].Trim();
                    blockedClients[userToBlock] = clientEndPoint; // key(string) = value(IpEndPoint)
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{GetClientName(clientEndPoint)} blocked {userToBlock}!");
                    Console.ResetColor();
                }
                break;
            case "unblock":
                if (messageParts.Length == 2 && GetClientName(clientEndPoint) == adminName && clients.Values.Contains(messageParts[1].Trim())) // Trim() - уникаємо у перевірці "\r\n"
                {
                    var userToUnblock = messageParts[1].Trim();
                    // out _ —> відкидає значення видаленого елемента(коли воно не потрібно, важливе лише саме видалення)
                    if (blockedClients.TryRemove(userToUnblock, out _))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine($"Admin unblocked {userToUnblock}");
                        Console.ResetColor();
                    }
                }
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Magenta;
                string messageOnServer = $"[{GetClientName(clientEndPoint)}]: {message}";
                Console.WriteLine(messageOnServer);
                Console.ResetColor();
                break;
        }
    }

    private void SendMessage(string sender, string recipient, string message, UdpClient udpServer)
    {
        var senderEndPoint = clients.FirstOrDefault(c => c.Value == sender).Key;

        if (IsBlocked(senderEndPoint, sender))
        {
            var blockedMessage = Encoding.UTF8.GetBytes("Rejected: You have been blocked!");
            udpServer.Send(blockedMessage, blockedMessage.Length, senderEndPoint);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"{sender} is trying to write something");
            Console.ResetColor();
            return;
        }

        foreach (var client in clients)
        {
            if (client.Value == recipient)
            {
                var fullMessage = Encoding.UTF8.GetBytes($"[{sender}] -> [{recipient}]: {message}");
                udpServer.Send(fullMessage, fullMessage.Length, client.Key);
            }
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        string messageOnServer = $"[{sender}] -> [{recipient}]: {message}";
        Console.WriteLine(messageOnServer);
    }

    private void BroadcastMessage(string sender, string message, UdpClient udpServer)
    {
        var senderEndPoint = clients.FirstOrDefault(c => c.Value == sender).Key;

        if (IsBlocked(senderEndPoint, sender))
        {
            var blockedMessage = Encoding.UTF8.GetBytes("Rejected: You have been blocked!");
            udpServer.Send(blockedMessage, blockedMessage.Length, senderEndPoint);
            return;
        }

        var fullMessage = Encoding.UTF8.GetBytes($"[{sender}] -> [All]: {message}");
        foreach (var clientEndPoint in clients.Keys)
        {
            if (!clientEndPoint.Equals(senderEndPoint)) // Пропустити відправлення відправнику
            {
                udpServer.Send(fullMessage, fullMessage.Length, clientEndPoint);
            }
        }
        Console.ForegroundColor = ConsoleColor.Blue;
        string messageOnServer = $"[{sender}] -> [All]: {message}";
        Console.WriteLine(messageOnServer);
    }

    private string GetClientName(IPEndPoint clientEndPoint)
    {
        return clients.TryGetValue(clientEndPoint, out var userName) ? userName : null;
    }

    private bool IsBlocked(IPEndPoint clientEndPoint, string sender)
    {
        return blockedClients.TryGetValue(sender, out var blockerEP) && blockerEP.Equals(clientEndPoint);
    }
}