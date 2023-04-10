using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pronia.Entities.SliderModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Entities.PlantModels
{
    public class Plant:BaseEntity
    {
        public List<Image> Images { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public List<PlantCategory> PlantCategories { get; set; }
        public List<PlantTag > PlantTags { get; set; }
        public int DeliveryInfoId { get; set; }
        public PlantDeliveryInfo DeliveryInfo { get; set; }
        public string Description { get; set; }
        public List<PlantSizeColor> PlantSizeColors { get; set; }

        public Plant()
        {
            Images = new();
            PlantCategories = new();
            PlantTags = new();
            PlantSizeColors = new();
        }
    }
}
