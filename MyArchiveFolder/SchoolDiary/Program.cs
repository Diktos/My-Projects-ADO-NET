using SchoolDiary.Models;
using SchoolDiary.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

using (var dbContext = new SchoolDiaryDbContext())
{
    // 1. Запит де ми відбираємо учнів з їх класами і класними керівниками 

    Console.WriteLine();
    var Students_Classes = dbContext.Students
        .Include(s => s.Class)
        .Select(s => new
        {
            StudentId = s.Id,
            FullName = $"{s.FirstName} {s.LastName}",
            ClassName = s.Class.Name,
            ClassTeacher = s.ClassTeacher
        })
        .ToList();

    foreach (var student in Students_Classes)
    {
        Console.WriteLine($"ID: {student.StudentId}, Name: {student.FullName}, Class: {student.ClassName}, ClassTeacher: {student.ClassTeacher.Name}");
    }

    // 2. Отримати всіх учнів, які отримали оцінки вище 10, з їх предметами

    Console.WriteLine();
    var highGrades = dbContext.Grades
        .Where(g => g.Value > 10)
        .Select(g => new
        {
            StudentName = dbContext.Students.FirstOrDefault(s => s.Id == g.StudentId).FirstName + " " +
                          dbContext.Students.FirstOrDefault(s => s.Id == g.StudentId).LastName,
            SubjectName = dbContext.Subjects.FirstOrDefault(sub => sub.Id == g.SubjectId).Name,
            GradeValue = g.Value
        })
        .ToList();

    foreach (var grade in highGrades)
    {
        Console.WriteLine($"Student: {grade.StudentName}, Subject: {grade.SubjectName}, Grade: {grade.GradeValue}");
    }

    // 3. Отримати список усіх предметів із вчителями, які їх викладають

    Console.WriteLine();
    var subjectsWithTeachers = dbContext.Subjects
        .Include(s => s.Teacher)
        .Select(s => new
        {
            SubjectName = s.Name,
            TeacherName = s.Teacher.Name
        })
        .ToList();

    foreach (var subject in subjectsWithTeachers)
    {
        Console.WriteLine($"Subject: {subject.SubjectName}, Teacher: {subject.TeacherName}");
    }

    // 4. Визначити відвідуваність учня за останній місяць

    Console.WriteLine();
    var lastMonth = DateTime.Now.AddMonths(-1);
    var studentAttendance = dbContext.Attendances
        .Where(a => a.AttendanceDate >= lastMonth)
        .Select(a => new
        {
            StudentName = dbContext.Students.FirstOrDefault(s => s.Id == a.StudentId).FirstName + " " +
                            dbContext.Students.FirstOrDefault(s => s.Id == a.StudentId).LastName,
            SubjectName = dbContext.Subjects.FirstOrDefault(sub => sub.Id == a.SubjectId).Name,
            Date = a.AttendanceDate,
            IsPresent = a.IsPresent
        })
        .ToList();

    foreach (var attendance in studentAttendance)
    {
        Console.WriteLine($"Student: {attendance.StudentName}, Subject: {attendance.SubjectName}, Date: {attendance.Date.ToShortDateString()}, Present: {attendance.IsPresent}");
    }

    // 5. Знайти середній бал для кожного учня

    Console.WriteLine();
    var averageGrades = dbContext.Grades
       .GroupBy(g => g.StudentId)
       .Select(gr => new
       {
           StudentName = dbContext.Students.FirstOrDefault(s => s.Id == gr.Key).FirstName + " " +
                         dbContext.Students.FirstOrDefault(s => s.Id == gr.Key).LastName,
           AverageGrade = gr.Average(g => g.Value)
       })
       .ToList();

    foreach (var student in averageGrades)
    {
        Console.WriteLine($"Student: {student.StudentName}, Average Grade: {student.AverageGrade:F2}");
    }

    // 6. Отримати список класів із кількістю учнів у кожному

    Console.WriteLine();
    var classStudentCounts = dbContext.Classes
        .Select(c => new
        {
            ClassName = c.Name,
            StudentCount = dbContext.Students.Count(s => s.ClassId == c.Id)
        })
        .ToList();

    foreach (var classInfo in classStudentCounts)
    {
        Console.WriteLine($"Class: {classInfo.ClassName}, Students: {classInfo.StudentCount}");
    }
}

