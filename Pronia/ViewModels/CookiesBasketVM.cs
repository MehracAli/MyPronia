using Pronia.Entities;

namespace Pronia.ViewModels
{
    public class CookiesBasketVM
    {
        public decimal TotalPrice { get; set; }
        public  List<CookiesItemVM> cookiesItemVMs { get; set; }

        public CookiesBasketVM()
        {
            cookiesItemVMs = new();
        }
    }
}
