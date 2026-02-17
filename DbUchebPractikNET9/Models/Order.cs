namespace DbUchebPractikNET9.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int IdUser { get; set; }
        public User User { get; set; }

        public int IdDeliveryOption { get; set; }
        public DeliveryOption DeliveryOption { get; set; }

        public int IdOrderStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}