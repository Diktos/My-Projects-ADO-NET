using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDiary.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static Teacher Teacher1 => new Teacher { Id = 1, Name = "Артем Стецько" };
        public static Teacher Teacher2 => new Teacher { Id = 2, Name = "Софія Калина" };
        public static Teacher Teacher3 => new Teacher { Id = 3, Name = "Валентин Нотатка" };
        public static Teacher Teacher4 => new Teacher { Id = 4, Name = "Олег Кернасто" };
        public static Teacher Teacher5 => new Teacher { Id = 5, Name = "Владислав Іскра" };

        public static IEnumerable<Teacher> DefaultTeachers()
        {
            yield return Teacher1;
            yield return Teacher2;
            yield return Teacher3;
            yield return Teacher4;
            yield return Teacher5;
        }
    }
}
