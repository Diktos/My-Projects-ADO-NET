using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class Server
{
    // [0]: key-value; [1]: key-value
    static ConcurrentDictionary<string, TcpClient> clients = new ConcurrentDictionary<string, TcpClient>();

    public static void Main(string[] args)
    {
        var server = new Server();
        server.Start();
    }

    public void Start()
    {
        Task server = Task.Run(() =>
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var tcpListener = new TcpListener(ipAddress, 10000);
            tcpListener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                Console.WriteLine("Waiting for connection...");
                var tcpClient = tcpListener.AcceptTcpClient();

                Task.Run(() =>
                {
                    var clientHandler = new ClientHandler(tcpClient);
                    clientHandler.Handle();
                });
            }
        });
        server.Wait(); // Можна і  Console.ReadLine(); щоб воно не закривало вікно, не ішло у фонове виконання
        //Console.ReadLine();
    }

    private class ClientHandler
    {
        private readonly TcpClient _tcpClient;

        public ClientHandler(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }
        // Main block method
        public void Handle()
        {
            try
            {
                var stream = _tcpClient.GetStream();
                var buffer = new byte[1024];
                var data = new StringBuilder();
                int bytesRead = 0;
                while (true)
                {
                    // Читаєм повідомлення
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        data.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        if (data.ToString().EndsWith("\r\n")) { break; }
                    }

                    // Додаємо нового клієнта до нашого словника
                    var messageParts = data.ToString().Trim().Split(' ');
                    if (messageParts.Length == 2 && messageParts[0] == "connect")
                    {
                        var nameUser = messageParts[1];
                        if (clients.TryAdd(nameUser, _tcpClient))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{nameUser} connected!");
                            Console.ResetColor();
                            stream.Write(Encoding.UTF8.GetBytes("Welcome to our server!\r\n"), 0, "Welcome to our server!\r\n".Length);
                        }
                    }

                    if (clients.ContainsKey(messageParts[1]))
                    {
                        while (true)
                        {
                            data.Clear();
                            // Читаєм повідомлення
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                data.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                                if (data.ToString().EndsWith("\r\n")) break;
                            }
                            if (data.ToString().ToLower() == "exit") { break; }

                            messageParts = data.ToString().Trim().Split(' ');
                            if (messageParts.Length == 3 && messageParts[0] == "send")
                            {
                                // Отримуємо ім'я одержувача з строки
                                var recipientName = messageParts[1];
                                // Формуємо повідомлення
                                var messageForSending = string.Join(' ', messageParts, 2, messageParts.Length - 2);
                                // Відправка
                                SendMessage(GetClientName(_tcpClient), recipientName, messageForSending, true);
                                continue;
                            }
                            if (messageParts.Length == 2 && messageParts[0] == "sendall")
                            {
                                // Отримуємо ім'я одержувача з строки
                                var recipientName = messageParts[1];
                                // Формуємо повідомлення
                                var messageForSending = string.Join(' ', messageParts, 1, messageParts.Length - 1);
                                // Відправка
                                SendMessage(GetClientName(_tcpClient), recipientName, messageForSending, false);
                                continue;
                            }
                            // Сервер бачить некоректні повідомлення користувача також
                            var message = string.Join(' ', messageParts, 0, messageParts.Length);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            string messageOnServer = $"[{GetClientName(_tcpClient)}]: {message}";
                            Console.WriteLine(messageOnServer);
                            Console.ResetColor();
                        }
                    }
                }

            }

            finally
            {
                // out _ —> відкидає значення видаленого елемента(коли воно не потрібно, важливе лише саме видалення)
                // out var value -> вихідний параметр, який містить значення, що було видалене з колекції
                if (_tcpClient != null)
                {
                    var clientName = GetClientName(_tcpClient);
                    if (clientName != null)
                    {
                        clients.TryRemove(clientName, out var deletingUser);
                    }
                    _tcpClient.Close();
                }
            }
        }


        private static void SendMessage(string clientSender, string clientRecipient, string message, bool privateMsg)
        {
            if (privateMsg)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string messageOnServer = $"[{clientSender}] -> [{clientRecipient}]: {message}";
                Console.WriteLine(messageOnServer);

                foreach (var kvp in clients)
                {
                    if (kvp.Key == clientRecipient)
                    {
                        string messageForRecipient = $"[{clientSender}]: {message}";
                        var clientStream = kvp.Value.GetStream();
                        byte[] responseData = Encoding.UTF8.GetBytes(messageForRecipient);
                        clientStream.Write(responseData, 0, responseData.Length);
                        Console.ResetColor();
                        break;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                string messageOnServer = $"[{clientSender}] -> All: {message}";
                Console.WriteLine(messageOnServer);
                foreach (var kvp in clients)
                {
                    if (kvp.Key != clientRecipient)
                    {
                        string messageForRecipient = $"[{clientSender}] to all: {message}";
                        var clientStream = kvp.Value.GetStream();
                        byte[] responseData = Encoding.UTF8.GetBytes(messageForRecipient);
                        clientStream.Write(responseData, 0, responseData.Length);
                        Console.ResetColor();
                        break;
                    }
                }
            }
        }

        private static string GetClientName(TcpClient client)
        {
            foreach (var kvp in clients)
            {
                if (kvp.Value == client)
                {
                    return kvp.Key;
                }
            }
            return null;
        }
    }
}


