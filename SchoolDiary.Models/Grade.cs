using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDiary.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Value { get; set; }

        public static Grade Grade1 => new Grade { Id = 1, StudentId = Student.Student1.Id, SubjectId = Subject.UkrainianLanguage.Id, Value = 12 };
        public static Grade Grade2 => new Grade { Id = 2, StudentId = Student.Student2.Id, SubjectId = Subject.Biology.Id, Value = 10 };
        public static Grade Grade3 => new Grade { Id = 3, StudentId = Student.Student1.Id, SubjectId = Subject.Math.Id, Value = 11 };
        public static Grade Grade4 => new Grade { Id = 4, StudentId = Student.Student2.Id, SubjectId = Subject.Сhemistry.Id, Value = 1 };
        public static Grade Grade5 => new Grade { Id = 5, StudentId = Student.Student3.Id, SubjectId = Subject.HistoryOfUkraine.Id, Value = 8 };
        public static Grade Grade6 => new Grade { Id = 6, StudentId = Student.Student4.Id, SubjectId = Subject.Сhemistry.Id, Value = 7 };
        public static Grade Grade7 => new Grade { Id = 7, StudentId = Student.Student1.Id, SubjectId = Subject.Biology.Id, Value = 2 };
        public static Grade Grade8 => new Grade { Id = 8, StudentId = Student.Student3.Id, SubjectId = Subject.UkrainianLanguage.Id, Value = 12 };
        public static Grade Grade9 => new Grade { Id = 9, StudentId = Student.Student2.Id, SubjectId = Subject.Math.Id, Value = 8 };
        public static Grade Grade10 => new Grade { Id = 10, StudentId = Student.Student4.Id, SubjectId = Subject.English.Id, Value = 12 };

        public static IEnumerable<Grade> DefaultGrades()
        {
            yield return Grade1;
            yield return Grade2;
            yield return Grade3;
            yield return Grade4;
            yield return Grade5;
            yield return Grade6;
            yield return Grade7;
            yield return Grade8;
            yield return Grade9;
            yield return Grade10;
        }
    }

}
