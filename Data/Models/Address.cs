using System.ComponentModel.DataAnnotations;

namespace SmallRestaurant.Data.Models
{
  public class Address
  {
    public Guid    Id          { get; set; }

    [Required]  
    public string  Street      { get; set; } = "";

    [Required]
    public string  City        { get; set; } = "";

    [Required]
    public string  PostalCode  { get; set; } = "";

    public bool    IsDefault   { get; set; }

    [Required]
    public string  UserId      { get; set; } = "";
  }
}