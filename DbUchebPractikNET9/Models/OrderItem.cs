namespace DbUchebPractikNET9.Models
{
    using System.ComponentModel.DataAnnotations;

    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        public int IdOrder { get; set; }
        public Order Order { get; set; }

        public int IdTechnic { get; set; }
        public Technic Technic { get; set; }

        public decimal PricePerDay { get; set; }
        public int Quantity { get; set; }
    }
}