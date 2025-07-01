using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmallRestaurant.Data.Models
{
  public class Dish
  {
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name must be under 100 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description must be under 500 characters")]
    public string Description { get; set; } = null!;

    [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
    public decimal Price { get; set; }
    
    public string? ImageUrl { get; set; }

    public List<Comment> Comments { get; set; } = new();
  }
}