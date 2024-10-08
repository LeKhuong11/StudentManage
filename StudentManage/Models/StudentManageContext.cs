using Microsoft.EntityFrameworkCore;
using StudentManage.Models;

namespace StudentManage.Data
{
    public class StudentManageContext : DbContext
    {
        public StudentManageContext(DbContextOptions<StudentManageContext> options) : base(options) { }

        public DbSet<S_User> S_Users { get; set; }

        public DbSet<S_Student> S_Students { get; set; }
    }
}
