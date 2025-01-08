using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;




// <div> порахувати кількість слів </div> на www.google.com 

//IPAddress iPAddress = IPAddress.Parse("192.168.1.1");
//iPAddress = new IPAddress(new byte[] { 192, 168, 1, 1 }); // 32 bit

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
    var buffer = new byte[1024];
    socket.Send(data, SocketFlags.None);
    int bytes = 0;
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

