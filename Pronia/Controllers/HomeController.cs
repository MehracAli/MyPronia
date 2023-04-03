using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Entities.SliderModels;
//using Pronia.Models;
using System.Diagnostics;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        public ProniaDbContext _context { get; set; }

        public HomeController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.OrderBy(s=>s.Order)
                .Include(s=>s.Discount)
                    .Include(s=>s.Next)
                    .Include(s=>s.Previous).ToList();

            ViewBag.Relateds = _context.Plants
                                .Include(p => p.Images)
                                    .OrderByDescending(p => p.Id)
                                        .Take(8)
                                            .ToList();
            return View(sliders);
        }
    }
}