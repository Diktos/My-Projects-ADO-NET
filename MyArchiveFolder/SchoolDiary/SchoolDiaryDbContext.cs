using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolDiary.Models;

namespace SchoolDiary.Data
{
    public class SchoolDiaryDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public SchoolDiaryDbContext() { }

        public SchoolDiaryDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/School_Diary.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Наповнення бази даними
            modelBuilder.Entity<Student>().HasData(Student.DefaultStudents());
            modelBuilder.Entity<Teacher>().HasData(Teacher.DefaultTeachers());
            modelBuilder.Entity<Subject>().HasData(Subject.DefaultSubjects());
            modelBuilder.Entity<Class>().HasData(Class.DefaultClasses());
            modelBuilder.Entity<Grade>().HasData(Grade.DefaultGrades());
            modelBuilder.Entity<Attendance>().HasData(Attendance.DefaultAttendances());

        }
    }
}
