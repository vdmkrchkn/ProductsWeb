namespace ProductsWebApi.Models.Json
{
    public class Order
    {
        public long ProductId { get; set; }

        public int Warranty { get; set; }

        public string DeliveryDate { get; set; }

        public string TimeSlot { get; set; }
    }
}
