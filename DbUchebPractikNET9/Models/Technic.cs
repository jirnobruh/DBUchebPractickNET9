using System.ComponentModel.DataAnnotations.Schema;

namespace DbUchebPractikNET9.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Technic
    {
        [Key]
        public int TechnicID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int IdCategory { get; set; }
        public TechnicCategory Category { get; set; }

        public int IdStatus { get; set; }
        public TechnicStatus Status { get; set; }
        
        public decimal PricePerDay { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<TechnicalService> TechnicalServices { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}