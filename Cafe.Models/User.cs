using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime? Birthday { get; set; } // string (3) //byte
        public IEnumerable<UserRole>? UserRoles { get; set; } 

        public static User Admin = new User { Id = 1, Name=Cafe.Models.Role.Admin.Name, Password="12345" };
        public static User Waiter => new User { Id = 2, Name = "Waiter", Password = "12345", Birthday = DateTime.Parse("01.01.2000") };
        public static User Manager => new User { Id = 3, Name = "Manager", Password = "12345", Birthday = DateTime.Parse("01.01.2000") };
        public static User Barmen => new User { Id = 4, Name = "Barmen", Password = "12345", Birthday = DateTime.Parse("01.01.2000") };
        public static User Cook => new User { Id = 5, Name = "Cook", Password = "12345", Birthday = DateTime.Parse("01.01.2000") };

    }
}
