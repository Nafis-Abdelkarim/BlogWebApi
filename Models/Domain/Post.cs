using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlogWebApi.Models;

public partial class Post
{
    public Guid PostId { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
