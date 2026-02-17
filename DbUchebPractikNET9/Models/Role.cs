namespace DbUchebPractikNET9.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleTitle { get; set; }

        public ICollection<User> Users { get; set; }
    }

}