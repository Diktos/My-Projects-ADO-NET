

// Особливості UDP:

// Безз'єднаний — не потребує встановлення з'єднання.
// Швидкий — мінімальна затримка через відсутність перевірки доставки.
// Ненадійний — пакети можуть загубитися, дублюватися або прийти не в тому порядку.
// Легкий — менший накладний трафік, ніж у TCP.
// Обмежений розмір — один датаграм до 65,535 байтів.
// Використання — потокове відео, онлайн-ігри.


using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Linq;

public class UDPChat
{
    private const int Port = 10000; // Спільний порт
    private UdpClient udpClient;
    private IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, Port);
    private static ConcurrentDictionary<IPEndPoint, string> clients = new ConcurrentDictionary<IPEndPoint, string>();
    private static HashSet<IPEndPoint> blockedClients = new HashSet<IPEndPoint>();
    private static string nickname = default;

    public void Start()
    {
        udpClient = new UdpClient();
        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
        udpClient.EnableBroadcast = true;
        Console.Write("Enter your nickname: ");
        nickname = Console.ReadLine();

        Console.ForegroundColor = ConsoleColor.Green;
        BroadcastMessage($"connect {nickname}");
        Console.ResetColor();

        ListenForMessages(); // Почмнаємо окрім писання ще й одночасно слухати

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Welcome to the live chat!");
        Console.ResetColor();

        Console.WriteLine("Commands:");
        Console.WriteLine("/sma [message] - Send message to all");
        Console.WriteLine("/spm [nickname] [message] - Send private message");
        Console.WriteLine("/block [nickname] - Block user");
        Console.WriteLine("/unblock [nickname] - Unblock user");
        Console.WriteLine("/exit - Exit the chat");

        while (true)
        {
            string input = Console.ReadLine();
            if (input.ToLower() == "/exit") { break; }

            if (input.StartsWith("/sma "))
            {
                string messageWithoutCommand = input.Substring(5);
                BroadcastMessage($"/sma {nickname} {messageWithoutCommand}");
            }
            else if (input.StartsWith("/spm "))
            {
                string[] parts = input.Split(' ');
                if (parts.Length == 3)
                {
                    string recipient = parts[1];
                    string message = parts[2];
                    BroadcastMessage($"/spm {nickname} {recipient} {message}");
                }
                else
                {
                    Console.WriteLine("Incorrect syntax. Usage: /spm [nickname] [message]");
                }
            }
            else if (input.StartsWith("/block "))
            {
                string targetNickname = input.Substring(7);
                BlockUser(targetNickname);
            }
            else if (input.StartsWith("/unblock "))
            {
                string targetNickname = input.Substring(9);
                UnblockUser(targetNickname);
            }
            else if(input.StartsWith("/help "))
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("/sma [message] - Send message to all");
                Console.WriteLine("/spm [nickname] [message] - Send private message");
                Console.WriteLine("/block [nickname] - Block user");
                Console.WriteLine("/unblock [nickname] - Unblock user");
                Console.WriteLine("/exit - Exit the chat");
            }
            else
            {
                Console.WriteLine("Unknown command. Use /help for view the list of commands");
            }
        }
    }

    private void ListenForMessages()
    {
        Task.Run(() =>
        {
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, 0); // ip + port(тей, хто нам каже)
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);

                string[] parts = message.Split(' '); // Ділимо все повідомлення на частини
                if (parts.Length < 1)
                {
                    continue; // Якщо команду надіслано невірну - оптимізуємо
                } 
                if (blockedClients.Contains(remoteEP))
                {
                    continue; // Ігноруємо повідомлення від заблокованих клієнтів
                }

                string command = parts[0];
                if (command == "connect")
                {
                    clients[remoteEP] = parts[1]; // parts[1] - nickname
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{parts[1]} has joined the chat!");
                    Console.ResetColor();
                }
                else if (command == "/sma" && parts.Length >= 2)
                {
                    string sender = parts[1];
                    string content = string.Join(' ', parts, 1, parts.Length - 1);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{sender}] -> [All]: {content}");
                    Console.ResetColor();
                }
                else if (command == "/spm" && parts.Length >= 3)
                {
                    string sender = parts[1];
                    string recipient = parts[2];
                    string content = string.Join(' ', parts, 2, parts.Length - 2);
                    if (recipient == nickname)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[{sender}] -> [You]: {content}");
                        Console.ResetColor();
                    }
                    
                }
            }
        });
    }

    private void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, broadcastEP);
    }

    private void BlockUser(string nickname)
    {
        var clientToBlock = clients.FirstOrDefault(c => c.Value == nickname);
        if (clientToBlock.Key != null)
        {
            blockedClients.Add(clientToBlock.Key); // Додаємо просто айпі заблокованого у 
            Console.WriteLine($"{nickname} has been blocked");
        }
        else
        {
            Console.WriteLine($"User {nickname} not found");
        }
    }

    private void UnblockUser(string nickname)
    {
        var blockedClient = clients.FirstOrDefault(c => c.Value == nickname);
        if (!blockedClient.Equals(default(KeyValuePair<IPEndPoint, string>)) && blockedClients.Remove(blockedClient.Key))
        {
            Console.WriteLine($"{nickname} has been unblocked");
        }
        else
        {
            Console.WriteLine($"User {nickname} not found");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var chat = new UDPChat();
        chat.Start();
    }
}