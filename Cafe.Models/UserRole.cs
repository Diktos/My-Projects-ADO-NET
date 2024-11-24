using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public User? User { get; set; }
        [ForeignKey(nameof(User))]
        public int WaiterId { get; set; } // WaiterId - бо у cafe_12.db WaiterId стовпчик має назву таку,
                                          // і я не знаю як змінити його назву
        public Role? Role { get; set; }
        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        public UserRole()
        {
            RoleId = 3;
        }
    }
}
