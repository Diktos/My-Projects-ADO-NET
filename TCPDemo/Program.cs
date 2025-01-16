
using System.Net;
using System.Net.Sockets;
using System.Text;

void StartServer()
{
    var ipAddress = IPAddress.Parse("127.0.0.1");
    var tcpListener = new TcpListener(ipAddress, 10000);
    tcpListener.Start();
    Console.WriteLine("Waiting for connection...");

    while (true)
    {
        var tcpClient = tcpListener.AcceptTcpClient();
        Console.WriteLine("Client connected");

        Task.Run(() => HandleFromServerForClient(tcpClient));
    }
}

void HandleFromServerForClient(TcpClient tcpClient)
{
    var stream = tcpClient.GetStream();
    var buffer = new byte[1024];
    var data = new StringBuilder();
    int bytes = 0;

    do
    {
        bytes = stream.Read(buffer, 0, buffer.Length);
        data.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
    }
    while (!data.ToString().EndsWith("EOF\r\n"));

    Console.WriteLine($"Data from client: {data}");

    string response = "Bye\r\n";
    var responseData = Encoding.UTF8.GetBytes(response);
    stream.Write(responseData, 0, responseData.Length);
    tcpClient.Close();
}


void Client(string clientName)
{
    var tcpClient = new TcpClient("127.0.0.1", 10000);

    var message = "Hello from client\r\nEOF\r\n";
    byte[] data = Encoding.UTF8.GetBytes(message);
    NetworkStream stream = tcpClient.GetStream();
    stream.Write(data, 0, data.Length);
    Console.WriteLine($"{clientName} sent: {message}");

    data = new byte[1024];
    var bytes = stream.Read(data, 0, data.Length);
    var response = Encoding.UTF8.GetString(data, 0, bytes);
    Console.WriteLine($"{clientName} received from server: {response}");
    stream.Close();
    tcpClient.Close();
}



Task.Run(() => StartServer());

Task[] clientTasks = new Task[5];

for (int i = 0; i < 5; i++)
{
    clientTasks[i] = Task.Run(() => Client($"Client {i + 1}"));
}
Console.ReadLine();