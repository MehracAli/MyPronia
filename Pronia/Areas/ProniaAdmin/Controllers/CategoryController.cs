using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Pronia.DAL;
using Pronia.Entities.PlantModels;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class CategoryController : Controller
    {
        public ProniaDbContext _context;
        public CategoryController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories.AsEnumerable();
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Creating(Category createdCategory)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "You cannot duplicate category name");
                return View();
            }
            bool duplicate = _context.Categories.Any(c => c.Name == createdCategory.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate category name");
                return View();
            }
            _context.Categories.Add(createdCategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            if (Id == 0) return NotFound();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == Id);
            if (category is null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult Editing(Category editedCategory, int Id)
        {
            if (Id != editedCategory.Id) return BadRequest();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == Id);
            if (category is null) return NotFound();
            bool duplicate = _context.Categories.Any(c => c.Name == editedCategory.Name && editedCategory.Name != category.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate category name");
                return View();
            }
            category.Name = editedCategory.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            if (Id == 0) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == Id);
            if (category is null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Deleting(int Id, Category deletedCategory)
        {
            if (Id != deletedCategory.Id) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == Id);
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
