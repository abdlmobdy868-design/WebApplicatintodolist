using Microsoft.AspNetCore.Mvc;
using WebApplicationtodolist.Data;
using WebApplicationtodolist.Models;
using Microsoft.EntityFrameworkCore;
public class TodoItemController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public TodoItemController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // 1. List + Search
    public async Task<IActionResult> Index(string searchString)
    {
        var todos = from t in _context.TodoItems select t;
        if (!string.IsNullOrEmpty(searchString))
            todos = todos.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
        return View(await todos.ToListAsync());
    }

    // 2. Create
    public IActionResult Create()
    {
        return View(new TodoItem());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoItem todo, IFormFile file)
    {
        if (file != null)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                await file.CopyToAsync(stream);
            todo.FilePath = fileName;
        }

        _context.Add(todo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // 3. Edit
    public async Task<IActionResult> Edit(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        return View(todo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TodoItem todo, IFormFile file)
    {
        if (file != null)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                await file.CopyToAsync(stream);
            todo.FilePath = fileName;
        }

        _context.Update(todo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // 4. Delete + تأكيد
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // 5. Download
    public IActionResult Download(string fileName)
    {
        var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
        return File(System.IO.File.ReadAllBytes(path), "application/octet-stream", fileName);
    }
}
