using System.ComponentModel.DataAnnotations;

namespace ApiRedis.Models;

public class Products
{
  [Key]
  public int ProductId { get; set; }
  public string ProductName { get; set; }
  public string ProductDescription { get; set; }
  public int Stock { get; set; }
}