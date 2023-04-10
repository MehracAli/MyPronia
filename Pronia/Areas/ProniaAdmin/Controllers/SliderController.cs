using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using Pronia.DAL;
using Pronia.Entities.SliderModels;
using Pronia.Utilities.Extentions;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SliderController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(ProniaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _context.Sliders.AsEnumerable();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Creating(Slider createdSlider)
        {
            if (createdSlider.Image is null)
            {
                ModelState.AddModelError("Image", "You must choose image here");
            }

            if (!createdSlider.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image", "You must coose image formats as: jpg, png etc.");
            }

            if(createdSlider.Image.IsValidLength(2))
            {
                ModelState.AddModelError("Image", "Image size cant be over 2MB");
            }


            string imagesFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            createdSlider.PlantImage = await createdSlider.Image.CreateImage(imagesFolderPath, "website-images");

            _context.Sliders.Add(createdSlider);
            _context.SaveChanges();

            return Json(createdSlider);
            //return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int Id)
        {
            if (Id == 0) return NotFound();
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == Id);
            if (slider == null) return NotFound();

            return View(slider);
        }
    }
}
