namespace DbUchebPractikNET9.Models;

using System.ComponentModel.DataAnnotations;

public class CartItem
{
    [Key]
    public int CartItemID { get; set; }
    public int IdCart { get; set; }
    public Cart Cart { get; set; }
    public int IdTechnic { get; set; }
    public Technic Technic { get; set; }
    public int Quantity { get; set; }
}