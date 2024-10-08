using System.ComponentModel.DataAnnotations;

namespace StudentManage.Models
{
    public class S_Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Class { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
