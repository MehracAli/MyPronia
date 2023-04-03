using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Entities.PlantModels;
using Pronia.ViewModels;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Pronia.Controllers
{
    public class PlantComparer : IEqualityComparer<Plant>
    {
        public bool Equals(Plant? x, Plant? y)
        {
            if (Equals(x?.Id, y?.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Plant obj)
        {
            throw new NotImplementedException();
        }
    }

    public class PlantCategoryComparer : IEqualityComparer<PlantCategory>
    {
        public bool Equals(PlantCategory? x, PlantCategory? y)
        {
            if (Equals(x?.Categories.Id, y?.Categories.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] PlantCategory obj)
        {
            throw new NotImplementedException();
        }
    }

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

            IQueryable<Plant> plants = _context.Plants.AsNoTracking().AsQueryable();
            
            Plant? plant = plants
                .Include(p=>p.PlantTags).ThenInclude(pt=>pt.Tags)
                    .Include(p=>p.Images)
                        .Include(p=>p.DeliveryInfo)
                            .Include(p => p.PlantCategories)
                                .ThenInclude(pc => pc.Categories).AsSingleQuery().FirstOrDefault(p => p.Id == Id);

            if (plant == null) return NotFound();

            ViewBag.Relateds = RelatedPlants(plants, plant, Id);

            return View(plant);
        }

        public List<Plant> RelatedPlants(IQueryable<Plant> plants, Plant plant, int Id)
        {
            List<Plant> relateds = new();

            plant.PlantCategories.ForEach(pc =>
            {
                List<Plant> related = plants.Include(p=>p.Images)
                 .Include(p => p.PlantCategories)
                            .ThenInclude(pc => pc.Categories)
                                .AsEnumerable()
                                        .Where(
                                            p => p.PlantCategories.Contains(pc, new PlantCategoryComparer())
                                            && p.Id != Id
                                            && !relateds.Contains(p, new PlantComparer())
                                        )
                                        .ToList();
                relateds.AddRange(related);
            });
            return relateds;
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
