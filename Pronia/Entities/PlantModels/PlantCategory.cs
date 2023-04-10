namespace Pronia.Entities.PlantModels
{
    public class PlantCategory:BaseEntity
    {
        public int CategoriesId { get; set; }
        public Plant Plants { get; set; }
        public Category Categories { get; set; }
    }
}
