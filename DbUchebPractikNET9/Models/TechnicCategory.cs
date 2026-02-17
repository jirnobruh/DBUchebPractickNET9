namespace DbUchebPractikNET9.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TechnicCategory
    {
        [Key]
        public int TechnicCategoryID { get; set; }
        public string CategoryTitle { get; set; }
        public string Description { get; set; }

        public ICollection<Technic> Technics { get; set; }
    }
}