using System.Net.Sockets;
using System.Net;
using System.Text;

public class UDPDemo_Client_chatProgram
{
    public static void Main(string[] args)
    {
        var client = new UDPDemo_Client_chatProgram();
        client.Start();
    }

    public void Start()
    {
        UdpClient udpClient = new UdpClient();
        // Необхідно для підтримки широкомовної розсилки повідомлень у мережі
        udpClient.EnableBroadcast = true;
        IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, 10000);

        Console.Write("Enter your name: ");
        var name = Console.ReadLine();
        var connectMessage = Encoding.UTF8.GetBytes($"connect {name}");
        udpClient.Send(connectMessage, connectMessage.Length, broadcastEP);

        var remoteEP = new IPEndPoint(IPAddress.Any, 0);
        var data = udpClient.Receive(ref remoteEP);
        var serverMessage = Encoding.UTF8.GetString(data);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(serverMessage);
        Console.ResetColor();

        Task.Run(() =>
        {
            while (true)
            {
                remoteEP = new IPEndPoint(IPAddress.Any, 0);
                data = udpClient.Receive(ref remoteEP);
                serverMessage = Encoding.UTF8.GetString(data);
                Console.WriteLine();
                Console.WriteLine(serverMessage);
            }
        });

        while (true)
        {
            var inputMessage = Console.ReadLine();
            if (string.IsNullOrEmpty(inputMessage)) { continue; }
            if (inputMessage.ToLower() == "exit") { break; }

            var messageData = Encoding.UTF8.GetBytes(inputMessage);
            udpClient.Send(messageData, messageData.Length, broadcastEP);
        }
    }
}