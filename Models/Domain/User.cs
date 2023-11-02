using System;
using System.Collections.Generic;

namespace BlogWebApi.Models.Domain;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public DateTime? Registered { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Role? Role { get; set; }
}
