﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Customer : BaseModel, ISoftDelete, IIdentity
    {
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsRegisted { get; set; } = false;

        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }

        public Gender Gender { get; set; } = Gender.Unknown;

        public IEnumerable<Address> Addresses { get; private set; }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     throw new NotImplementedException();
        // }
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
