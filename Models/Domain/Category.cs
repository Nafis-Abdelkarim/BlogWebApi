using System;
using System.Collections.Generic;
using BlogWebApi.Models.Domain;

namespace BlogWebApi.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
