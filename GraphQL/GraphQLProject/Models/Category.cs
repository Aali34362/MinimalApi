﻿namespace GraphQLProject.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty!;
    public string ImageUrl { get; set; } = string.Empty!;
    public virtual ICollection<Menu>? Menus { get; set; }
}
