using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMS.Data;
using SPMS.Models;

namespace SPMS.Controllers;

public class StudentsController : Controller
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search)
    {
        var query = _db.Students.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search) || s.Email.Contains(search) || s.Course.Contains(search));
        }
        ViewBag.Search = search;
        return View(await query.OrderByDescending(s => s.RegistrationDate).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await _db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        return student is null ? NotFound() : View(student);
    }

    [HttpGet]
    public IActionResult Create() => View(new Student { DateOfBirth = DateTime.Today.AddYears(-18) });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (await _db.Students.AnyAsync(s => s.Email == student.Email))
            ModelState.AddModelError(nameof(student.Email), "A student with this email already exists.");

        if (!ModelState.IsValid) return View(student);
        student.RegistrationDate = DateTime.UtcNow;
        student.IsActive = true;
        _db.Students.Add(student);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Student registered successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _db.Students.FindAsync(id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id) return BadRequest();
        if (await _db.Students.AnyAsync(s => s.Email == student.Email && s.Id != id))
            ModelState.AddModelError(nameof(student.Email), "Another student is already using this email.");
        if (!ModelState.IsValid) return View(student);
        _db.Update(student);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Student updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is not null)
        {
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Student deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
