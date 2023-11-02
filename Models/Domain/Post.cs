using System;
using System.Collections.Generic;

namespace BlogWebApi.Models.Domain;

public partial class Post
{
    public int PostId { get; set; }

    public int? CategoryId { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
