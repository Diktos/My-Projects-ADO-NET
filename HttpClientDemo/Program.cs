using HttpClientDemo;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Json;

//var client = new HttpClient();
//client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
//client.DefaultRequestHeaders.Accept.Clear();
//client.DefaultRequestHeaders.Accept.Add(
//    new MediaTypeWithQualityHeaderValue("application/json"));
//client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
//await cllient.PostAsJsonAsync("posts", new { title = "foo", body = "bar", userId = 1 }); // POST https://jsonplaceholder.typicode.com/posts

//var responce = await client.PostAsync("posts/1"); // GET https://jsonplaceholder.typicode.com/posts/1


//var response = await client.GetAsync("posts"); // GET https://jsonplaceholder.typicode.com/posts



var client = new HttpClient();
client.BaseAddress = new Uri("https://sabatex.francecentral.cloudapp.azure.com");
var login = new { clientId = new Guid("c8a41470-25d3-4f2e-9dc6-1cb9955587d1"), password = "bigbos228" };

var responce = await client.PostAsJsonAsync("api/v0/login", login); // POST https://sabatex.francecentral.cloudapp.axure.com/api/auth/login

if (responce.IsSuccessStatusCode)
{
    var content = await responce.Content.ReadAsStringAsync();
    Token token = System.Text.Json.JsonSerializer.Deserialize<Token>(content);
    client.DefaultRequestHeaders.Add("clientId", "c8a41470-25d3-4f2e-9dc6-1cb9955587d1");
    client.DefaultRequestHeaders.Add("destinationId", "2a2c2a72-0ecb-4369-8e1e-c84be1f201ee");
    client.DefaultRequestHeaders.Add("apiToken", token.AccessToken);

    var objectValue = new { messageHeader = "Test", message = "Hello from client1", dateStamp=DateTime.Now };
    var responce2 = await client.PostAsJsonAsync("api/v1/objects", objectValue); // POST https://sabatex.francecentral.cloudapp.axure.com/api/object
    if (responce2.IsSuccessStatusCode)
    {
        var content2 = await responce2.Content.ReadAsStringAsync();
        Console.WriteLine(content2);
    }
    else
    {
        Console.WriteLine($"Failed to get data, status code: {responce2.StatusCode}");
    }


    Console.WriteLine(content);
}
else
{
    Console.WriteLine($"Failed to get data, status code: {responce.StatusCode}");
}
Console.ReadLine();



//var responce = await client.GetAsync("/"); // GET https://www.google.com/
//if (responce.IsSuccessStatusCode)
//{
//    var content = await responce.Content.ReadAsStringAsync();
//    Console.WriteLine(content);
//}
//else
//{
//    Console.WriteLine($"Failed to get data, status code: {responce.StatusCode}");
//}


//WebClient client = new WebClient();  // NET Framework 3.5 (4.0) 