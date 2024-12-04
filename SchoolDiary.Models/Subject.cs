using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDiary.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public static Subject Math => new Subject { Id = 1, Name = "Математика", TeacherId=Teacher.Teacher1.Id };
        public static Subject HistoryOfUkraine => new Subject { Id = 2, Name = "Історія України", TeacherId = Teacher.Teacher2.Id };
        public static Subject English => new Subject { Id = 3, Name = "Англійська мова", TeacherId = Teacher.Teacher3.Id };
        public static Subject Biology => new Subject { Id = 4, Name = "Біологія", TeacherId = Teacher.Teacher4.Id };
        public static Subject UkrainianLanguage => new Subject { Id = 5, Name = "Українська мова", TeacherId = Teacher.Teacher5.Id };
        public static Subject Сhemistry => new Subject { Id = 6, Name = "Сhemistry", TeacherId = Teacher.Teacher2.Id };

        public static IEnumerable<Subject> DefaultSubjects()
        {
            yield return Math;
            yield return HistoryOfUkraine;
            yield return English;
            yield return Biology;
            yield return UkrainianLanguage;
            yield return Сhemistry;
        }
    }
}
