namespace DbUchebPractikNET9.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TechnicStatus
    {
        [Key]
        public int TechnicStatusID { get; set; }
        public string StatusTitle { get; set; }

        public ICollection<Technic> Technics { get; set; }
    }
}