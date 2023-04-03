namespace Pronia.Entities.PlantModels
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public List<PlantCategory> PlantCategories { get; set; }
        public Category()
        {
            PlantCategories = new();
        }
    }
}
