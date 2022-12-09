namespace Products.Web.Core.Models
{
    public class Order : BaseJson
    {
        public long ProductId { get; set; }

        public int Warranty { get; set; }

        public string DeliveryDate { get; set; }

        public string TimeSlot { get; set; }
    }
}
