using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class Currency
    {
        [Key] // Entity Framework вимагає, щоб усі сутності мали первинний ключ (зазвичай Id - автоматично сприймає за нього, але у нас випадок,
              // коли він має іншу назву і це треба вказати явно, що "Code" - первинний ключ
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
