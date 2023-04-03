namespace Pronia.Entities.PlantModels
{
    public class PlantDeliveryInfo:BaseEntity
    {
        public string Shipping { get; set; }
        public string AboutReturnRequest { get; set; }
        public string Guarantee { get; set; }
        public List<Plant> Plants { get; set; }

        public PlantDeliveryInfo()
        {
            Plants = new();
        }
    }
}
