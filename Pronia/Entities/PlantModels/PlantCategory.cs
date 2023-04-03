namespace Pronia.Entities.PlantModels
{
    public class PlantCategory:BaseEntity
    {
        public Plant Plants { get; set; }
        public Category Categories { get; set; }
    }
}
