using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolDiary.Models;

namespace SchoolDiary.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ClassId { get; set; }
        public Class? Class { get; set; }
        public int ClassTeacherId { get; set; }
        public Teacher? ClassTeacher { get; set; }

        public static Student Student1 => new Student { Id = 1, FirstName = "Данило", LastName = "Міхневич", ClassId = Class.Class8B.Id, ClassTeacherId = Teacher.Teacher1.Id };
        public static Student Student2 => new Student { Id = 2, FirstName = "Стас", LastName = "Мікроволновка", ClassId = Class.Class8B.Id, ClassTeacherId = Teacher.Teacher1.Id };
        public static Student Student3 => new Student { Id = 3, FirstName = "Антон", LastName = "Потік", ClassId = Class.Class11A.Id, ClassTeacherId = Teacher.Teacher2.Id };
        public static Student Student4 => new Student { Id = 4, FirstName = "Марина", LastName = "Сталкер", ClassId = Class.Class10B.Id, ClassTeacherId = Teacher.Teacher3.Id };
        public static Student Student5 => new Student { Id = 5, FirstName = "Богдан", LastName = "Компот", ClassId = Class.Class11A.Id, ClassTeacherId = Teacher.Teacher4.Id };
        public static Student Student6 => new Student { Id = 6, FirstName = "Софія", LastName = "Роментік", ClassId = Class.Class6A.Id, ClassTeacherId = Teacher.Teacher2.Id };

        public static IEnumerable<Student> DefaultStudents()
        {
            yield return Student1;
            yield return Student2;
            yield return Student3;
            yield return Student4;
            yield return Student5;
            yield return Student6;
        }
    }
}
