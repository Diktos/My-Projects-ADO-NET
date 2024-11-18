using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public string OrderDate { get; set; }
        public int OrderId { get; set; }
        public int NomenclatureId { get; set; }
        public double Price { get; set; }
        public double Count { get; set; }
        public double Sum { get; set; }
    }
}
