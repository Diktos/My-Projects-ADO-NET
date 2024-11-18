using CW_13._11_EF_Core;
using Cafe.Models;
using (var context = new CafeDbContext())
{
    foreach(var waiter in context.Users.ToArray())
    {
        Console.WriteLine($"{waiter.Id} {waiter.Name} {waiter.Password} {waiter.Birthday}");
    }
}