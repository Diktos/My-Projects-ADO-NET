using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDiary.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Class Class11A => new Class { Id = 1, Name = "11-А" };
        public static Class Class10B => new Class { Id = 2, Name = "10-Б" };
        public static Class Class9B => new Class { Id = 3, Name = "9-Б" };
        public static Class Class8B => new Class { Id = 4, Name = "8-Б" };
        public static Class Class7A => new Class { Id = 5, Name = "7-A" };
        public static Class Class6A => new Class { Id = 6, Name = "6-A" };

        public static IEnumerable<Class> DefaultClasses()
        {
            yield return Class11A;
            yield return Class10B;
            yield return Class9B;
            yield return Class8B;
            yield return Class7A;
            yield return Class6A;
        }
    }
}
