namespace Pronia.Entities.SliderModels
{
    public class RightIcon:BaseEntity
    {
        public string IconClass { get; set; }
        public List<Slider> Sliders { get; set; }
        public RightIcon()
        {
            Sliders = new();
        }
    }
}
