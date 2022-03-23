using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Web.Infrastructure.Entities
{
    public enum OrderStatus
    {
        Registered = 1,
        Accepted, // принят на складе;
        InProgress, // в пути;
        Delivered,
        Done,
        Rejected,
    }

    [Table("Orders")]
    public class OrderEntity : BaseEntity
    {
        public OrderStatus Status { get; set; }

        public string PostOffice { get; set; }

        public IEnumerable<ProductEntity> Products { get; set; }

        public decimal TotalPrice { get; set; }

        public string ClientFullname { get; set; }

        public string Phone { get; set; }

        public int Warranty { get; set; }

        public string DeliveryDate { get; set; }

        public string TimeSlot { get; set; }
    }
}
