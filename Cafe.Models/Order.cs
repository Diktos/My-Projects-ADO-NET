using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public ClientTable? ClientTable { get; set; }
        public int ClientTableId { get; set; }
        //[NotMapped]
        //[Column("ITOG")]
        //[Precision(16,2)]    // 2345.56  real/double  (numeric/decimal)
        public double Bill { get; set; }

        //[NotMapped]
        //[JsonIgnore]
        //public IEnumerable<OrderDetail>? Details { get; set; }

        public static Order Order1 => new Order { Id = 1, ClientTableId = ClientTable.Table1.Id, UserId = User.Waiter.Id, Date = new DateTime(2024, 3, 2, 9, 30, 23), Bill = 0 };
        public static Order Order2 => new Order { Id = 2, ClientTableId = ClientTable.Table2.Id, UserId = User.Waiter.Id, Date = new DateTime(2024, 3, 2, 9, 50, 23), Bill = 0 };
        public static Order Order3 => new Order { Id = 3, ClientTableId = ClientTable.Table3.Id, UserId = User.Waiter.Id, Date = new DateTime(2024, 3, 2, 9, 5, 23), Bill = 0 };


        public static IEnumerable<Order> DefaultOrders()
        {
            yield return Order1;
            yield return Order2;
            yield return Order3;
        }
    }
}
