namespace DbUchebPractikNET9.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }

        public int IdRole { get; set; }
        public Role Role { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<TechnicalService> TechnicalServices { get; set; }
    }
}