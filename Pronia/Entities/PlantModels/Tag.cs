namespace Pronia.Entities.PlantModels
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public List<PlantTag> PlantTags { get; set; }
        public Tag()
        {
            PlantTags = new();
        }
    }
}
