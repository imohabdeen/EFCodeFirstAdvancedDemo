using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstAdvancedDemo
{
    public class StudentEntityConfiguration : EntityTypeConfiguration<Student>
    {
        //When you have a large number of domain classes, then configuring every class in OnModelCreating can 
        //become unmanageable. Code-First enables you to move all the configurations related to one domain class to a separate class
        public StudentEntityConfiguration()
        {

            this.ToTable("StudentInfo");

            this.HasKey<int>(s => s.StudentID);


            this.Property(p => p.DateOfBirth)
                    .HasColumnName("DoB")
                    .HasColumnOrder(3)
                    .HasColumnType("datetime2");

            this.Property(p => p.StudentName)
                    .HasMaxLength(50);

            this.Property(p => p.StudentName)
                    .IsConcurrencyToken();

            this.HasMany<Course>(s => s.Courses)
                .WithMany(c => c.Students)
                .Map(cs =>
                {
                    cs.MapLeftKey("StudentId");
                    cs.MapRightKey("CourseId");
                    cs.ToTable("StudentCourse");
                });
        }
    }
}
