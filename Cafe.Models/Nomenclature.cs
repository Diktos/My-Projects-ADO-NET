using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
     public class Nomenclature
    {
        public int Id { get; set; }
        [MaxLength(100)]
        //[Column(TypeName ="varchar(200)")]
        [Display(Name = "Name")]
        [StringLength(50, MinimumLength = 5)]
        //[RegularExpression(@"^[А-ЩЬЮЯҐЄІЇ]{2}\d{6}$",ErrorMessage = "The passport number is incorrect.")] // CL4545OK
        //[RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$", ErrorMessage = "The Car number is incorect. Please enter numbrer as AB1234RT.")]
        public string Name { get; set; } = default!;
        [Display(Name = "Price")]
        public double Price { get; set; }

        public static Nomenclature Soup1 => new Nomenclature { Id = 1, Name = "Суп гороховий", Price = 45 };
        public static Nomenclature Soup2 => new Nomenclature { Id = 2, Name = "Суп сирний", Price = 75 };
        public static Nomenclature Soup3 => new Nomenclature { Id = 3, Name = "Суп харчо", Price = 55 };
        public static Nomenclature Locy => new Nomenclature { Id = 4, Name = "Лоці", Price = 105 };
        public static Nomenclature Potatoes => new Nomenclature { Id = 5, Name = "Картопля фрі", Price = 60 };


        public static IEnumerable<Nomenclature> DefaultNomenclatures()
        {
            yield return Soup1;
            yield return Soup2;
            yield return Soup3;
            yield return Locy;
            yield return Potatoes;
        }

        void Test()
        {
            foreach (var nom in DefaultNomenclatures())
            {

            }
        }
    }

}
