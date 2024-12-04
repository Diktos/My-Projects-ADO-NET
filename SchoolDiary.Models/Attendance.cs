using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDiary.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public Student? Student { get; set; }
        public int StudentId { get; set; }
        public Subject? Subject { get; set; }
        public int SubjectId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }

        public static Attendance Attendance1 => new Attendance
        {
            Id = 1,
            StudentId = Student.Student1.Id,
            SubjectId = Subject.UkrainianLanguage.Id,
            AttendanceDate = new DateTime(2024, 10, 10),
            IsPresent = true
        };
        public static Attendance Attendance2 => new Attendance
        {
            Id = 2,
            StudentId = Student.Student2.Id,
            SubjectId = Subject.English.Id,
            AttendanceDate = new DateTime(2024, 12, 2),
            IsPresent = false
        };
        public static Attendance Attendance3 => new Attendance
        {
            Id = 3,
            StudentId = Student.Student1.Id,
            SubjectId = Subject.Biology.Id,
            AttendanceDate = new DateTime(2024, 11, 3),
            IsPresent = true
        };
        public static Attendance Attendance4 => new Attendance
        {
            Id = 4,
            StudentId = Student.Student6.Id,
            SubjectId = Subject.Сhemistry.Id,
            AttendanceDate = new DateTime(2024, 12, 1),
            IsPresent = true
        };
        public static Attendance Attendance5 => new Attendance
        {
            Id = 5,
            StudentId = Student.Student3.Id,
            SubjectId = Subject.HistoryOfUkraine.Id,
            AttendanceDate = new DateTime(2024, 12, 1),
            IsPresent = false
        };
        public static Attendance Attendance6 => new Attendance
        {
            Id = 6,
            StudentId = Student.Student4.Id,
            SubjectId = Subject.Math.Id,
            AttendanceDate = new DateTime(2024, 12, 2),
            IsPresent = true
        };

        public static IEnumerable<Attendance> DefaultAttendances()
        {
            yield return Attendance1;
            yield return Attendance2;
            yield return Attendance3;
            yield return Attendance4;
            yield return Attendance5;
            yield return Attendance6;
        }
    }

}
