using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserRole>? UserRoles { get; set; }

        public static readonly Role Admin = new Role { Id=1, Name="Admin" };
        public static readonly Role Manager = new Role { Id = 2, Name = "Manager"};
        public static readonly Role User = new Role { Id = 3, Name = "User"};
    }
}

