namespace DbUchebPractikNET9.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DeliveryOption
    {
        [Key]
        public int DeliveryOptionID { get; set; }

        public string OptionTitle { get; set; }
        public string Description { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}