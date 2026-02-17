namespace DbUchebPractikNET9.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Cart
{
    [Key]
    public int CartID { get; set; }

    public int IdUser { get; set; }
    public User User { get; set; }

    public ICollection<CartItem> CartItems { get; set; }
}