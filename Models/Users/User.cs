using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
  public class User : BaseModel, ISoftDelete, IIdentity
  {
    [Required]
    public string Avatar { get; set; }
    [Required]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    public byte[] Password { get; set; }
    public byte[] PasswordSalt { get; set; }
    [Column(TypeName = "date")]
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; } = Gender.Unknown;
    public string Phone { get; set; }
    public string Address { get; set; }
    public IEnumerable<UserRole> UserRoles { get; set; }
    [Required]
    public bool IsDeleted { get; set; } = false;
  }
}
