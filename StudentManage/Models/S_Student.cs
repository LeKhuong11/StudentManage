using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManage.Models
{
    public class S_Student
    {
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; } = "0";

        [Required]
        public string Name { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Class { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
