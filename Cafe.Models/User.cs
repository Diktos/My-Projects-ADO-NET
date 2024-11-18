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
        public DateTime Birthday { get; set; } // string (3) //byte
        public IEnumerable<UserRole> UserRoles { get; set; }

    }
}
