using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class Order
    {
        public  string Date { get; set; }
        public int Id { get; set; }
        [ForeignKey(nameof(Waiter))]
        public int WaiterId { get; set; }
        [ForeignKey(nameof(ClientTable))]
        public int ClientTableId { get; set; }
        public double Bill {  get; set; } // Рахунок
    }
}
