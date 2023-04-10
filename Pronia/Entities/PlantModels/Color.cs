namespace Pronia.Entities.PlantModels
{
    public class Color:BaseEntity
    {
        public string Name { get; set; }
        public List<PlantSizeColor> PlantSizeColors { get; set; }

        public Color()
        {
            PlantSizeColors = new();
        }
    }
}
