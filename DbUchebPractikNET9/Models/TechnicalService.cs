namespace DbUchebPractikNET9.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TechnicalService
    {
        [Key]
        public int TechnicalServiceID { get; set; }

        public int IdTechnic { get; set; }
        public Technic Technic { get; set; }

        public DateTime TsDate { get; set; }
        public string Description { get; set; }

        public int IdPerformedUser { get; set; }
        public User PerformedUser { get; set; }
    }
}