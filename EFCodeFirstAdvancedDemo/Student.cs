using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstAdvancedDemo
{
    public class Student
    {
        public Student()
        {

        }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Photo { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }
        public int StandardId { get; set; }

        public Standard Standard { get; set; }
        public virtual StudentAddress Address { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
