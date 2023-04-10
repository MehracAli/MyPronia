using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Entities.PlantModels;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class TagController : Controller
    {
        public ProniaDbContext _context;

        public TagController(ProniaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Tag> tags = _context.Tags.AsEnumerable();
            return View(tags);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Creating(Tag createdTag)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "You cannot duplicate tag name");
                return View();
            }
            bool duplicate = _context.Tags.Any(t => t.Name == createdTag.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate tag name");
                return View();
            }
            _context.Tags.Add(createdTag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            if (Id == 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
            if (tag is null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult Editing(Tag editedTag, int Id)
        {
            if (Id != editedTag.Id) return BadRequest();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
            if (tag is null) return NotFound();
            bool duplicate = _context.Categories.Any(c => c.Name == editedTag.Name && editedTag.Name != tag.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate tag name");
                return View();
            }
            tag.Name = editedTag.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            if (Id == 0) return NotFound();
            Tag? tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
            if (tag is null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Deleting(int Id, Tag deletedTag)
        {
            if (Id != deletedTag.Id) return NotFound();
            Tag? tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
            if (tag is null) return NotFound();
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
