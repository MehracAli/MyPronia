using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pronia.Entities.PlantModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.ViewModels
{
    public class PlantVM
    {
        public int Id { get; set; }
        [StringLength(maximumLength:20)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int PlantDeliveryInfoId { get; set; }
        [NotMapped]
        public ICollection<int> CategoriesIds { get; set; } = null;
        [NotMapped]
        public ICollection<int> TagsIds { get; set; } = null;
        public ICollection<int> ColorIds { get; set; } = null;
        public ICollection<int> SizeIds { get; set; } = null;
        public ICollection<PlantSizeColor> PlantSizeColors { get; set; } = null;
        [NotMapped]
        public ICollection<IFormFile>? Images { get; set; }
        public List<Image> SpecificImages { get; set; }
        public List<int> ImageIds { get; set; }
        [NotMapped]
        public IFormFile IsMainImage { get; set; } = null;
        [NotMapped]
        public IFormFile HoverImage { get; set; } = null;
        public string? ColorSizeQuantity { get; set; }
    }
}
