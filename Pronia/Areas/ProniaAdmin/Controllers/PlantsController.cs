using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Entities.PlantModels;
using Pronia.Utilities.Extentions;
using Pronia.ViewModels;
using System.Linq;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class PlantsController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantsController(ProniaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Plant> plants = _context.Plants.Include(p => p.Images).AsNoTracking().AsEnumerable();
            return View(plants);
        }

        public IActionResult Create()
        {
            ViewBag.PlantDeliveryInfo = _context.PlantDeliveryInfos.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlantVM createdPlant)
        {
            ViewBag.PlantDeliveryInfo = _context.PlantDeliveryInfos.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            TempData["InvalidImages"] = string.Empty;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ok");
                return View();
            }
            //return Json(createdPlant.HoverImage);
            if (!createdPlant.IsMainImage.IsValidFile("image/") || !createdPlant.HoverImage.IsValidFile("image"))
            {
                ModelState.AddModelError("","Invalid file type");
                return View();
            }
            if (!createdPlant.IsMainImage.IsValidLength(2) || !createdPlant.HoverImage.IsValidLength(2))
            {
                ModelState.AddModelError("","File size cannot be over 2MB");
                return View();
            }

            Plant plant = new()
            {
                Name = createdPlant.Name,
                Description = createdPlant.Description,
                Price = createdPlant.Price,
                SKU = createdPlant.SKU,
                DeliveryInfoId = createdPlant.PlantDeliveryInfoId
            };
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");


            foreach (IFormFile image in createdPlant.Images)
            {
                if(!image.IsValidFile("image/") || image.IsValidLength(2))
                {
                    TempData["InvalidImages"] += image.FileName;
                    continue;
                }
                Image plantImage = new()
                {
                    IsMain = false,
                    ImagePath = await image.CreateImage(imageFolderPath, "website-images"),
                };
                plant.Images.Add(plantImage);
            }

            Image isMain = new()
            {
                IsMain = true,
                ImagePath = await createdPlant.IsMainImage.CreateImage(imageFolderPath, "website-images"),
            };
            plant.Images.Add(isMain);

            Image isHover = new()
            {
                IsMain = false,
                ImagePath = await createdPlant.IsMainImage.CreateImage(imageFolderPath, "website-images"),
            };
            plant.Images.Add(isHover);
            foreach (int id in createdPlant.CategoriesIds)
            {
                PlantCategory plantCategory = new()
                {
                    CategoriesId = id,
                    //Plants = plant
                };
                plant.PlantCategories.Add(plantCategory);
            }

            foreach (int id in createdPlant.TagsIds)
            {
                PlantTag plantTag = new()
                {
                    TagsId = id,
                    //Plants = plant
                };
                plant.PlantTags.Add(plantTag);
            }
            _context.Plants.Add(plant);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if(id == 0) return BadRequest();

            PlantVM? plantVM = _context.Plants
                .Include(p => p.PlantCategories)
                    .Include(p => p.PlantTags)
                        .Include(p => p.Images)
                            .Include(p=>p.PlantSizeColors)
                .Select(p => new PlantVM {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    SKU = p.SKU,
                    Description = p.Description,
                    PlantDeliveryInfoId = p.DeliveryInfoId,
                    CategoriesIds = p.PlantCategories.Select(pc => pc.CategoriesId).ToList(),
                    TagsIds = p.PlantTags.Select(pt => pt.TagsId).ToList(),
                    ColorIds = p.PlantSizeColors.Select(pc => pc.ColorId).ToList(),
                    SizeIds = p.PlantSizeColors.Select(pc => pc.SizeId).ToList(),
                    PlantSizeColors = p.PlantSizeColors.ToList(),
                    SpecificImages = p.Images.Select(p => new Image
                    {
                        Id = p.Id,
                        IsMain = p.IsMain,
                        ImagePath = p.ImagePath,
                    }).ToList()
                })
                .FirstOrDefault(p => p.Id == id);

            if (plantVM == null) return BadRequest();

            ViewBag.PlantDeliveryInfos = _context.PlantDeliveryInfos.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            ViewBag.Sizes = _context.Sizes.AsEnumerable();
            ViewBag.Quantites = _context.PlantSizeColors.Include(psc=>psc.Color)
                                                            .Include(psc => psc.Size)
                                                                .AsEnumerable();

            return View(plantVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, PlantVM editedPlant)
        {
            if(Id == 0) return BadRequest();
            Plant? plant = _context.Plants
                .Include(p=>p.Images)
                    .Include(p=>p.PlantCategories)
                        .Include(p=>p.PlantTags)
                            .FirstOrDefault(p => p.Id == Id);
            if(plant == null) return BadRequest();

            IEnumerable<string> removables = plant.Images
                .Where(i => !editedPlant.ImageIds.Contains(i.Id))
                    .Select(i=>i.ImagePath)
                        .AsEnumerable();

            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            foreach (string removable in removables)
            {
                string path = Path.Combine(imageFolderPath, "website-images", removable);
                FileUpload.DeleteImage(path);
            }

            if (editedPlant.IsMainImage is not null)
            {
                string imagesFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
                string filePath = Path.Combine(imagesFolderPath, "website-images", plant.Images.FirstOrDefault(i=>i.IsMain is true).ImagePath);
                FileUpload.DeleteImage(filePath);
                plant.Images.FirstOrDefault(i => i.IsMain is true).ImagePath = await editedPlant.IsMainImage.CreateImage(imagesFolderPath, "website-images");
            }

            plant.Description = editedPlant.Description;
            plant.Price = editedPlant.Price;
            plant.SKU = editedPlant.SKU;



            foreach (int id in editedPlant.CategoriesIds)
            {
                PlantCategory plantCategory = new()
                {
                    CategoriesId = id,
                    //Plants = plant
                };
                plant.PlantCategories.RemoveAll(pc => !editedPlant.CategoriesIds.Contains(pc.CategoriesId));
                plant.PlantCategories.Add(plantCategory);
            }

            foreach (int id in editedPlant.TagsIds)
            {
                PlantTag plantTag = new()
                {
                    TagsId = id,
                    //Plants = plant
                };

                plant.PlantTags.RemoveAll(pt => !editedPlant.TagsIds.Contains(pt.TagsId));
                plant.PlantTags.Add(plantTag);
            }


            plant.Images.RemoveAll(i=> !editedPlant.ImageIds.Contains(i.Id));
            _context.SaveChanges();
            return RedirectToAction("Edit", "Plants") ;
        }
    }
}
