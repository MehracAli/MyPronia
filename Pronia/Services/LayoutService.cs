using Newtonsoft.Json;
using NuGet.ContentModel;
using Pronia.DAL;
using Pronia.Entities;
using Pronia.Entities.PlantModels;
using Pronia.ViewModels;

namespace Pronia.Services
{
    public class LayoutService
    {
        public ProniaDbContext _context { get; set; }
        public IHttpContextAccessor _accessor { get; set; }

        public LayoutService(ProniaDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

        public CookiesBasketVM GetBasket()
        {
            var cookies = _accessor.HttpContext.Request.Cookies["basket"];
            CookiesBasketVM basket = new();
            if (cookies is not null)
            {
                basket = JsonConvert.DeserializeObject<CookiesBasketVM>(cookies);
                foreach (CookiesItemVM item in basket.cookiesItemVMs)
                {
                    Plant plant = _context.Plants.FirstOrDefault(p=>p.Id == item.Id);
                    if (plant is null)
                    {
                        basket.cookiesItemVMs.Remove(item);
                        basket.TotalPrice -= item.Price*item.Quantity;
                    }
                    GetBasketItem(item.Id);
                }
            }
            return basket;
        }

        public Plant GetBasketItem(int id)
        {
            var cookies = _accessor.HttpContext.Request.Cookies["basket"];
            List<Plant> plants = _context.Plants.ToList();
            CookiesBasketVM basket = new();
            if (cookies is not null)
            {
                basket = JsonConvert.DeserializeObject<CookiesBasketVM>(cookies);
                foreach (var item in basket.cookiesItemVMs)
                {
                   Plant plant = plants.FirstOrDefault(p => p.Id == id);
                    if(plant is not null) {
                        return plant;
                    }
                }
            }
            return null;
        }

        public void RemoveItem(int id)
        {

        }
    }
}
