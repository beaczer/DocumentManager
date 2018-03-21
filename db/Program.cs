using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db
{
    class Program
    {
        static void Main(string[] args)
        {
            using(ctx= new Data)
        }
        public class Student
        {
            public Student()
            {

            }
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public byte[] Photo { get; set; }
            public decimal Height { get; set; }
            public float Weight { get; set; }

            public Teacher Teacher { get; set; }

            public Standard Standard { get; set; }
        }

        public class Teacher
        {
            public Teacher()
            {

            }
            public int TeacherId { get; set; }
            public string TeacherName { get; set; }
        }
    
        public class SchoolContext : DbContext
        {
            public SchoolContext() : base()
            {

            }

            public DbSet<Student> Students { get; set; }
            public DbSet<Standard> Standards { get; set; }

        }
        public class Standard
        {
            public Standard()
            {

            }
            public int StdId { get; set; }
            public string StandardName { get; set; }

            public IList<Student> Students { get; set; }

        }

    }

}
}
