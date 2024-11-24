using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebsiteGenZ.Models;

[Serializable]
public class CartItemModel
{
    [Key]
    public int Id { get; set; } 

    public int CartId { get; set; } 
    [ForeignKey("CartId")]
    public CartModel Cart { get; set; } 

    public int ProductId { get; set; } 
    [ForeignKey("ProductId")]
    public ProductModel Product { get; set; }

    public int Quantity { get; set; } 
}
