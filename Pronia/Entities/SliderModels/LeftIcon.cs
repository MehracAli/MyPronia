namespace Pronia.Entities.SliderModels
{
    public class LeftIcon : BaseEntity
    {
        public string IconClass { get; set; }
        public List<Slider> Sliders { get; set; }
        public LeftIcon()
        {
            Sliders = new();
        }
    }
}
