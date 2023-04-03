namespace Pronia.Entities.PlantModels
{
    public class PlantTag:BaseEntity
    {
        public Plant Plants { get; set; }
        public Tag Tags { get; set; }
    }
}
