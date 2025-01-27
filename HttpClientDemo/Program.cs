using HttpClientDemo;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

// HTTP (HyperText Transfer Protocol) — це протокол для передачі даних у вебмережі.
// Він використовується для зв’язку між ВЕББРАУЗЕРОМ (клієнтом) і СЕРВЕРОМ.

// Використовується для перегляду вебсторінок або отримання ресурсів із серверів сайтів.


// Багатопоточний словник для зберігання користувачів: ["Ім'я"] = "API", key = value
var clients = new ConcurrentDictionary<string, string>
{
    ["Serhiy.Lakas"] = "2a2c2a72-0ecb-4369-8e1e-c84be1f201ee", 
    ["Kostyikevych.Vitaliy"] = "76efc553-f68d-4aa9-b1af-c25a6dfb0390",
    ["Shalovilo.Bogdan"] = "1134a432-9005-4232-b0e9-8ead9cf63c0b",
    ["Nikolaichyk.Vlad"] = "fdb887f6-61ee-48b9-992f-6fff53207a3c",
    ["Andryii.Rabosh"] = "7e23a154-09d2-4ab8-b23a-4ca24181e1ef",
    ["Iskachov.Bohdan"] = "b3f1b17d-8caa-4c74-87ab-b53c67788e98",
    ["Zakusilo.Vitaliy"] = "b862de90-f671-4f96-a8aa-154841d18b95",
    ["Bobrovnitskiy.Matviy"] = "b934d65c-83e7-4f74-834e-94fc12004ad7"
};
var messageDatabase = new List<Message>(); // Зберігаємо всі Message(SenderName, SenderApi, Text)

var client = new HttpClient
{
    BaseAddress = new Uri("https://sabatex.francecentral.cloudapp.azure.com")
};

string ObjectsRoute = "api/v1/objects";

await LoginToLiveChat(); // Логінимося
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Welcome to the live chat!");
Console.ResetColor();
Console.WriteLine("Commands:");
Console.WriteLine("/sma [message] - Send a message to all users");
Console.WriteLine("/spm [name] [message] - Send a private message");
Console.WriteLine("/help - All available commands in the chat");
Console.WriteLine("/exit - Exit the chat");


// Починаємо слухання повідомлень
Task listenTask = ListeningProcess(client, clients, ObjectsRoute);

// Основний потік для спілкування (написання повідомлень користувачем)
while (true)
{
    string input = Console.ReadLine();

    if (input.ToLower() == "/exit") { break; }

    if (input.StartsWith("/sma "))
    {
        string message = input.Substring(5);
        await BroadcastMessage(client, clients, message, "BroadcastMessage");
    }
    else if (input.StartsWith("/spm "))
    {
        string[] parts = input.Split(' ', 3); // Після ім'я користувача і пробілу, все інше - повідомлення 

        string recipientName = parts[1];
        string message = parts[2];

        if (clients.TryGetValue(recipientName, out string recipientApi)) // Якщо ми знайшли користувача з таким ім'ям(тру) - отримаємо його Апі(value) 
        {
            await SendMessage(client, recipientApi, message, "Test");
        }
        else
        {
            Console.WriteLine($"User {recipientName} not found");
        }
    }
    else if (input.StartsWith("/help"))
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("/sma [message] - Send a message to all users");
        Console.WriteLine("/spm [name] [message] - Send a private message");
        Console.WriteLine("/help - All available commands in the chat");
        Console.WriteLine("/exit - Exit the chat");
    }
    else
    {
        Console.WriteLine("Unknown command. Use /help for view the list of commands");
    }
}

// Метод для broadcast-повідомлення
async Task BroadcastMessage(HttpClient client, ConcurrentDictionary<string, string> clients, string message, string messageHeader)
{
    foreach (var clientFromList in clients)
    {
        try
        {
            var destination = clientFromList.Value; // API кожного користувача
            var result = client.DefaultRequestHeaders.SingleOrDefault(s => s.Key == "destinationId");
            if (result.Key != null)
            {
                client.DefaultRequestHeaders.Remove("destinationId");
            }
            client.DefaultRequestHeaders.Add("destinationId", destination);

            var objectValue = new
            {
                messageHeader = messageHeader,
                message = message,
                dateStamp = DateTime.Now
            };

            var messageToSend = await client.PostAsJsonAsync(ObjectsRoute, objectValue);
            if (!messageToSend.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to send message to {clientFromList.Key}, status code: {messageToSend.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message to {clientFromList.Key}: {ex.Message}");
        }
    }
}


async Task ListeningProcess(HttpClient client, ConcurrentDictionary<string, string> clients, string objectsRoute)
{
    //int count = 1;
    DateTime lastReceivedDate = DateTime.MinValue; // Початкова дата для фільтрації нових повідомлень
    bool isFirstRun = true; // Флаг для першого запуску
    await Task.Run(async () =>
    {
        while (true)
        {
            try
            {
                foreach (var clientFromList in clients)
                {
                    try
                    {
                        var destination = clientFromList.Value; // API кожного користувача
                        var result = client.DefaultRequestHeaders.SingleOrDefault(s => s.Key == "destinationId");
                        if (result.Key != null)
                        {
                            client.DefaultRequestHeaders.Remove("destinationId");
                        }
                        client.DefaultRequestHeaders.Add("destinationId", destination);

                        var response = await client.GetAsync(objectsRoute);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var messages = JsonSerializer.Deserialize<List<Message>>(content);

                            foreach (var m in messages.Take(10)) // Тіки 10 зараз може прийти
                            {
                                if (isFirstRun)
                                {
                                    var userName = clients.SingleOrDefault(s => s.Value == m.Sender).Key;
                                    Console.WriteLine($"[{userName}] ({m.DateStamp:dd-MM-yyyy HH:mm:ss}): {m.TextMessage}");

                                    messageDatabase.Add(m);

                                    continue;
                                }

                                else if(m.DateStamp > lastReceivedDate)
                                {
                                    var userName = clients.SingleOrDefault(s => s.Value == m.Sender).Key;
                                    Console.WriteLine($"[{userName}] ({m.DateStamp:dd-MM-yyyy HH:mm:ss}): {m.TextMessage}");

                                    messageDatabase.Add(m);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine($"Error sending message to {clientFromList.Key}: {ex.Message}");
                    }
                }
                // Якщо це перший запуск, встановлюємо флаг в false, щоб далі отримувати лише нові повідомлення
                if (isFirstRun)
                {
                    isFirstRun = false;
                }
                lastReceivedDate = DateTime.Now;

            }
            catch (Exception ex) // Щоб бачити яка натуральна помилка при слуханні(перешкоди). Вимкнено, бо буде спам
            {
                //Console.WriteLine(ex.Message);
            }

            await Task.Delay(2000); // Пауза між отриманням повідомлень
        }
    });
}


async Task SendMessage(HttpClient client, string destination, string message, string messageHeader)
{
    var result = client.DefaultRequestHeaders.SingleOrDefault(s => s.Key == "destinationId");
    if (result.Key!=null)
    {
        client.DefaultRequestHeaders.Remove("destinationId");
    }
    client.DefaultRequestHeaders.Add("destinationId", destination); // Апі того кому відправляю

    var objectValue = new
    {
        messageHeader = messageHeader,
        message = message,
        dateStamp = DateTime.Now
    };

    var response = await client.PostAsJsonAsync(ObjectsRoute, objectValue);
    if (response.IsSuccessStatusCode)
    {
        //var content = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(content);
    }
    else
    {
        Console.WriteLine($"Failed to send message, status code: {response.StatusCode}");
    }
}

async Task LoginToLiveChat()
{
    var login = new { clientId = new Guid("c8a41470-25d3-4f2e-9dc6-1cb9955587d1"), password = "bigbos228" };
    var responce = await client.PostAsJsonAsync("api/v0/login", login); // POST https://sabatex.francecentral.cloudapp.axure.com/api/auth/login
    if (responce.IsSuccessStatusCode)
    {
        var content = await responce.Content.ReadAsStringAsync();
        Token token = System.Text.Json.JsonSerializer.Deserialize<Token>(content);
        client.DefaultRequestHeaders.Add("clientId", "c8a41470-25d3-4f2e-9dc6-1cb9955587d1"); // Мій апі для відправки 
        client.DefaultRequestHeaders.Add("apiToken", token.AccessToken);
        Console.WriteLine("Login successfully!");
    }
    else
    {
        Console.WriteLine("Login unsuccessfully!");
    }
}



public class Message
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("sender")]
    public string Sender { get; set; }
    [JsonPropertyName("destination")]
    public string Destination { get; set; }
    [JsonPropertyName("messageHeader")]
    public string MessageHeader { get; set; }
    [JsonPropertyName("dateStamp")]
    public DateTime DateStamp { get; set; }
    [JsonPropertyName("senderDateStamp")]
    public DateTime SenderDateStamp { get; set; }
    [JsonPropertyName("message")]
    public string TextMessage { get; set; }

    public Message(int id, string sender, string destination, string messageHeader, DateTime dateStamp, DateTime senderDateStamp, string textMessage)
    {
        Id = id;
        Sender = sender;
        Destination = destination;
        MessageHeader = messageHeader;
        DateStamp = dateStamp;
        SenderDateStamp = senderDateStamp;
        TextMessage = textMessage;
    }
}
