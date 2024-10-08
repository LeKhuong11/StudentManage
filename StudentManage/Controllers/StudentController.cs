using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManage.Data;
using StudentManage.Models;
using System.Security.Claims;

namespace StudentManage.Controllers
{
    [Route("Student")]
    [Authorize]
    public class StudentController : Controller
    {
        private readonly StudentManageContext _context;

        public StudentController(StudentManageContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        
        [HttpGet("Index")]
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

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(S_Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
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


        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Class")] S_Student student)
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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _context.S_Students.SingleOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.S_Students.SingleOrDefault(s => s.Id == id);
            if (student != null)
            {
                _context.S_Students.Remove(student);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
