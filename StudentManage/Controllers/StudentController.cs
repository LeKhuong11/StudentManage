using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManage.Data;
using StudentManage.Models;

namespace StudentManage.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly StudentManageContext _context;

        public StudentController(StudentManageContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        
        public IActionResult Index()
        {
            var students = _context.S_Students.ToList();

            return View(students);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // Nếu thêm [HttpPost("Create")] trước hàm Create trong StudentController,
        // có nghĩa là phương thức này chỉ có thể được gọi bằng một yêu cầu HTTP POST, và không phải bằng yêu cầu GET.
        // Nếu cố gắng truy cập sẽ nhận được lỗi 404
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Gender,Phone,Class")] S_Student student)
        {
            if (ModelState.IsValid)
            {
                student.StudentId = GenerateStudentId();
                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student created successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }


        [HttpGet("Edit")]
        public IActionResult Edit(int id)
        {
            var student = _context.S_Students.SingleOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        // Task: Đây là một đại diện cho một công việc bất đồng bộ (asynchronous operation).
        // Sử dụng Task cho phép bạn thực hiện các công việc như truy vấn cơ sở dữ liệu, gọi API,
        // hoặc thực hiện bất kỳ công việc nào có thể mất thời gian mà không chặn luồng chính của ứng dụng.
        // IActionResult: Đây là một giao diện trong ASP.NET Core đại diện cho kết quả của một hành động (action).
        // IActionResult có thể đại diện cho nhiều loại kết quả khác nhau, chẳng hạn như trả về một trang HTML (View), một đối tượng JSON, hoặc một mã trạng thái HTTP.
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Name,Gender,Phone,Class")] S_Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Fetch the original entity from the database
                var existingStudent = await _context.S_Students.FindAsync(id);

                if (existingStudent == null)
                {
                    return NotFound();
                }

                // Update the properties
                existingStudent.Name = student.Name;
                existingStudent.Gender = student.Gender;
                existingStudent.Phone = student.Phone;
                existingStudent.Class = student.Class;

                try
                {
                    // Save the changes
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.S_Students.Any(e => e.Id == student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirect back to the list of students after a successful update
                return RedirectToAction(nameof(Index));
            }

            // Return the view with the same student data in case of validation errors
            return View(student);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = _context.S_Students.SingleOrDefault(s => s.Id == id);
            if (student != null)
            {
                _context.S_Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        private string GenerateStudentId()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}
