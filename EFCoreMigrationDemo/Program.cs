
using Cafe.Models;
using EFCore_Demo;
using Microsoft.EntityFrameworkCore;

using (var context = new CafeDbContext())
{
    var user = new User { Name = "User 1", Password = "********", Birthday = DateTime.Now };
    var role = new Role { Name = "Manager" };
    var userRole = new UserRole { Role = role, User = user };

    context.Add(user);
    context.Add(role);
    context.Add(userRole);
    context.SaveChanges();

    var users = context.Users.Include(i => i.UserRoles.ToArray());

    var userRoles = context.UserRoles.ToArray();

    var i = 1;

}
