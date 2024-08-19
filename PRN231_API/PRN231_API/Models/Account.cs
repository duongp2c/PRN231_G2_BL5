using System;
using System.Collections.Generic;

namespace PRN231_API.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? ActiveCode { get; set; }
        public bool IsActive { get; set; }

        public virtual Student? Student { get; set; }
        public virtual Teacher? Teacher { get; set; }
    }
}
