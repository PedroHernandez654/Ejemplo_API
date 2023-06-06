using System.ComponentModel.DataAnnotations;

namespace UniversityApiBackend.Models.DataModels
{
    public enum Level
    {
        Basic, Medium, Advance, Expert
    }
    public class Course:BaseEntity
    {
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(280)]
        public string ShortDescription { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public Level Level { get; set; } = Level.Basic;
        //Category
        [Required]
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        //Index
        [Required]
        public Chapters Chapters { get; set; } = new Chapters();
        //Student
        [Required]
        public ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
