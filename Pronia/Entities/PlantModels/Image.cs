namespace Pronia.Entities.PlantModels
{
    public class Image:BaseEntity
    {
        public string ImagePath { get; set; }
        public bool? IsMain { get; set; }
        public Plant Plant { get; set; }
    }
}
