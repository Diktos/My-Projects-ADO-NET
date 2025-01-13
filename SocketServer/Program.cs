using System.Net.Sockets;
using System.Net;
using System.Text;
//var ipAddres = IPAddress.Parse("127.0.0.1"); // localhost
IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 10000);
var socketListener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

try
{
    socketListener.Bind(iPEndPoint);
    socketListener.Listen(10); // Максимальна кількість очікувань підключень
    while (true)
    {
        Console.WriteLine($"Waiting for connection in {iPEndPoint} ...");
        Socket connectedSocket = socketListener.Accept();
        Console.WriteLine($"Connected from {connectedSocket.RemoteEndPoint}");

        //Task.Run(() =>
        //{ 
            // Масив для тимчасового зберігання отриманих байтів з сокета
            var buffer = new byte[1024];
            // Використовується для побудови кінцевого тексту відповіді, що надійшла від сервера
            var data = new StringBuilder();
            // Кількість байтів, які фактично були отримані під час одного виклику
            int bytes = 0;

        // Асинхронне слухання клієнта
        //Task receiveTask = Task.Run(() =>
        //{
        while (true)
        {
            data.Clear();
            bytes = 0;

            do
                {
                    bytes = connectedSocket.Receive(buffer, buffer.Length, 0);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                } while (!data.ToString().EndsWith("\r\n") || bytes<=0);
                    //}

                Console.WriteLine();
                Console.WriteLine($"Data from client: {data}");
                if (data.Equals("EOF"))
                {
                    break;
                }

                //  Повідомлень для кліэнта
                Console.Write("-->");
                string responseMessage = Console.ReadLine() + "\r\n";
                var responseData = Encoding.UTF8.GetBytes(responseMessage);
                connectedSocket.Send(responseData);
        }
                //});

                //Task sendTask = Task.Run(() =>
                //{
                //    while (true)
                //    {
                //Console.Write("-->");
                //string responseMessage = Console.ReadLine() + "\r\n";
                //var responseData = Encoding.UTF8.GetBytes(responseMessage);
                //connectedSocket.Send(responseData);

                //if (responseMessage.Equals("EOF"))
                //{
                //    break;
                //}
                //    }
                ////});

                //});
            }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
finally
{
    socketListener.Close();
}