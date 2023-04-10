namespace Pronia.Entities.PlantModels
{
    public class PlantTag:BaseEntity
    {
        public int TagsId { get; set; }
        public Plant Plants { get; set; }
        public Tag Tags { get; set; }
    }
}
