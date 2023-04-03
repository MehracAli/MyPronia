namespace Pronia.Entities.SliderModels
{
    public class Slider : BaseEntity
    {
        public string PlantImage { get; set; }
        public string PlantName { get; set; }
        public Discount Discount { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public LeftIcon Previous { get; set; }
        public RightIcon Next { get; set; }
    }
}
