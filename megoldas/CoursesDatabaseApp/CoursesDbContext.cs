using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesDatabaseApp
{
    internal class CoursesDbContext : DbContext
    {
        //public DbSet<Course> Courses { get; set; }
        //public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorCourse> InstructorCourses { get; set; }

        public CoursesDbContext() {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseInMemoryDatabase("CoursesDb");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstructorCourse>()
                .HasOne(ic => ic.Instructor)
                .WithMany(i => i.InstructorCourses)
                .HasForeignKey(ic => ic.InstructorId);

            modelBuilder.Entity<InstructorCourse>()
                .HasOne(ic => ic.Course)
                .WithMany(c => c.InstructorCourses)
                .HasForeignKey(ic => ic.CourseId);

            // Seed data for Instructors
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, Name = "Jane Doe", Neptun = "ABX21J" },
                new Instructor { Id = 2, Name = "John Smith", Neptun = "QR76K3" },
                new Instructor { Id = 3, Name = "Bela Vak", Neptun = "UOMI6X" },
                new Instructor { Id = 4, Name = "Main Iac", Neptun = "T67REX" },
                new Instructor { Id = 5, Name = "Charles Dufay", Neptun = "QUAX1P" }
            );

            // Seed data for Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "Math", Credits = 8 },
                new Course { Id = 2, Title = "English", Credits = 2 },
                new Course { Id = 3, Title = "Geography", Credits = 5 },
                new Course { Id = 4, Title = "Poetry", Credits = 6 },
                new Course { Id = 5, Title = "Drama", Credits = 5 },
                new Course { Id = 6, Title = "Social Studies", Credits = 4 }
            );

            // Seed data for InstructorCourses
            modelBuilder.Entity<InstructorCourse>().HasData(
                new InstructorCourse { Id = 1, InstructorId = 1, CourseId = 1 }, // Jane Doe teaches Math
                new InstructorCourse { Id = 2, InstructorId = 1, CourseId = 3 },  // Jane Doe teaches Geography
                new InstructorCourse { Id = 3, InstructorId = 3, CourseId = 2 }, // Bela Vak teaches English
                new InstructorCourse { Id = 4, InstructorId = 3, CourseId = 3 }, // Bela Vak teaches Geography
                new InstructorCourse { Id = 5, InstructorId = 3, CourseId = 6 }, // Bela Vak teaches Social Studies
                new InstructorCourse { Id = 6, InstructorId = 4, CourseId = 4 }, // Main Iac teaches Poetry
                new InstructorCourse { Id = 7, InstructorId = 2, CourseId = 5 }, // John Smith and Charles Dufay teach Drama
                new InstructorCourse { Id = 8, InstructorId = 5, CourseId = 5 } // John Smith and Charles Dufay teach Drama
            );



            base.OnModelCreating(modelBuilder);
        }
    }
}
