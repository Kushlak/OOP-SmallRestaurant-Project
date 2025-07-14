﻿namespace SmallRestaurant.Data.Models
{
  public class User
  {
    public Guid   Id           { get; set; } = Guid.NewGuid();
    public string UserName     { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role         { get; set; } = "User";  

    public List<Order> Orders  { get; set; } = new();
  }
}