using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Models;

namespace StudentRegisteration.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
    {

    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseOffering> CourseOfferings { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Mark> Marks { get; set; }
    public DbSet<StudentDetails> StudentDetails { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<EnrollmentCourse> EnrollmentCourses { get; set; }
    // public DbSet<CourseMark> CourseMarks  { get; set; }

// for many to many and making 2pk to 1 pk
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnrollmentCourse> ()
        .HasKey(ec => new { ec.EnrollmentId, ec.CourseId });
        
    }
}
