using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;
    [StringLength(10, ErrorMessage = "The name is limited to 10 letters.")]
    public string? FirstName { get; set; }
    [StringLength(20, ErrorMessage = "The name is limited to 20 letters.")]
    public string? LastName { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
