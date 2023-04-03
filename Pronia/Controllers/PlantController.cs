using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Entities.PlantModels;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class PlantController:Controller
    {
        ProniaDbContext _context { get; set; }

        public PlantController(ProniaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Plant> plants = _context.Plants.Include(p=>p.Images).ToList();
            return View(plants);
        }

        public IActionResult Detail(int Id)
        {
            if (Id == 0) return NotFound();
            Plant? plant = _context.Plants.Include(p => p.PlantCategories).ThenInclude(pc => pc.Categories)
                .Include(p=>p.PlantTags).ThenInclude(pt=>pt.Tags)
                .Include(p=>p.Images)
                .Include(p=>p.DeliveryInfo)
                .FirstOrDefault(p=>p.Id == Id);
            if (plant == null) return NotFound();

            ViewBag.Relateds = _context.Plants.Include(p=>p.Images)
                .Take(6)
                .ToList();
            
            return View(plant);
        }

        public IActionResult AddToBasket(int Id)
        {
            if (Id <= 0) return NotFound();
            Plant? plant = _context.Plants.FirstOrDefault(p => p.Id == Id);
            if (plant == null) return NotFound();

            var Cookies = HttpContext.Request.Cookies["basket"];
            CookiesBasketVM basket = new();

            if (Cookies is null)
            {
                CookiesItemVM item = new()
                {
                    Id = plant.Id,
                    Quantity = 1,
                    Price = plant.Price,
                };
                basket.cookiesItemVMs.Add(item);
                basket.TotalPrice += item.Price;
            }
            else
            {
                basket = JsonConvert.DeserializeObject<CookiesBasketVM>(Cookies);
                CookiesItemVM exictedItem = basket.cookiesItemVMs.Find(i=>i.Id == Id);
                if (exictedItem is null)
                {
                    CookiesItemVM item = new()
                    {
                        Id = plant.Id,
                        Quantity = 1,
                        Price = plant.Price,
                    };
                    basket.cookiesItemVMs.Add(item);
                    basket.TotalPrice += item.Price;
                }
                else
                {
                    exictedItem.Quantity++;
                    basket.TotalPrice += exictedItem.Price;
                }
            }
            var basketStr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("basket", basketStr);
            return RedirectToAction("Index","Home");
        }

        public IActionResult DeleteBasketItem(int Id)
        {
            var cookies = HttpContext.Request.Cookies["basket"];
            var basket = JsonConvert.DeserializeObject<CookiesBasketVM>(cookies);

            foreach (CookiesItemVM item in basket.cookiesItemVMs)
            {
                if (item.Id == Id)
                {
                    basket.cookiesItemVMs.Remove(item);
                    basket.TotalPrice -= item.Price*item.Quantity;
                    break;
                };
            }
            var basketStr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("basket", basketStr);
            return RedirectToAction("Index","Home");
        }
    }
}
