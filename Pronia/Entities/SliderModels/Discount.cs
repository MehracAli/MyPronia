namespace Pronia.Entities.SliderModels
{
    public class Discount : BaseEntity
    {
        public int DiscountDigit { get; set; }
        public List<Slider> Sliders { get; set; }
        public Discount()
        {
            Sliders = new();
        }
    }
}
