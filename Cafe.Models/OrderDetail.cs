using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public Nomenclature? Nomenclature { get; set; }
        public int NomenclatureId { get; set; }
        public double Price { get; set; }
        public double Count { get; set; }
        public double Sum { get; set; }
        public static IEnumerable<OrderDetail> Defaults()
        {
            yield return new OrderDetail { Id = 1, OrderId = Order.Order1.Id, NomenclatureId = Nomenclature.Soup1.Id, Count = 2, Price = Nomenclature.Soup1.Price, Sum = 2 * Nomenclature.Soup1.Price };
            yield return new OrderDetail { Id = 2, OrderId = Order.Order1.Id, NomenclatureId = Nomenclature.Locy.Id, Count = 2, Price = Nomenclature.Locy.Price, Sum = 2 * Nomenclature.Locy.Price };

        }
    }
}
