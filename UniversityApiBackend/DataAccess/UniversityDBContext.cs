using Microsoft.EntityFrameworkCore;
using UniversityApiBackend.Models.DataModels;

namespace UniversityApiBackend.DataAccess
{
    public class UniversityDBContext: DbContext
    {
        //Constructor
        public UniversityDBContext(DbContextOptions<UniversityDBContext> options): base(options) { }


        //Add Dbsets (Tables of our database)
        public DbSet<User>? Users { get; set; }
        public DbSet<Course>? Courses { get; set;}
        public DbSet<Chapters>? Chapters { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Student>? Students { get; set; }

    }
}
