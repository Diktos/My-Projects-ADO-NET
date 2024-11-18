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
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
