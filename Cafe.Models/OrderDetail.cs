using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string OrderDate { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        [ForeignKey(nameof(Nomenclature))]
        public int NomenclatureId { get; set; }
        public double Price { get; set; }
        public double Count { get; set; }
        public double Sum { get; set; }
    }
}
