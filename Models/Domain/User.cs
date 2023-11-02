using System;
using System.Collections.Generic;
using BlogWebApi.Models.Domain;

namespace BlogWebApi.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid? RoleId { get; set; }

    public DateTime? Registered { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Role? Role { get; set; }
}
