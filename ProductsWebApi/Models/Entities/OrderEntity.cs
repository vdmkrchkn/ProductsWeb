using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsWebApi.Models.Entities
{
    [Table("Orders")]
    public class OrderEntity : BaseEntity
    {
        public long ProductId { get; set; }

        public int Warranty { get; set; }

        public string DeliveryDate { get; set; }

        public string TimeSlot { get; set; }
    }
}
