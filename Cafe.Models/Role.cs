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

        public static Role Admin => new Role { Id = 1, Name = "Admin" };
        public static Role Manager => new Role { Id = 2, Name = "Manager" };
        public static Role Waiter => new Role { Id = 3, Name = "Waiter" };
    }
}

