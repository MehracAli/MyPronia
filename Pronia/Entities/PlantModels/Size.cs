namespace Pronia.Entities.PlantModels
{
    public class Size:BaseEntity
    {
        public string Name { get; set; }
        public List<PlantSizeColor> PlantSizeColors { get; set; }
        
        public Size()
        {
            PlantSizeColors = new();
        }
    }
}
