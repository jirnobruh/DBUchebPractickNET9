namespace DbUchebPractikNET9.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderStatus
    {
        [Key]
        public int OrderStatusID { get; set; }
        public string StatusTitle { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}