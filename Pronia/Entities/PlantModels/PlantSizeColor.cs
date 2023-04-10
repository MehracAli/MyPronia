namespace Pronia.Entities.PlantModels
{
    public class PlantSizeColor : BaseEntity
    {
        //Id's
        public int PlantId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        //References
        public Plant Plant { get; set; } = null;
        public Color Color { get; set; } = null;
        public Size Size { get; set; } = null;
    }
}
