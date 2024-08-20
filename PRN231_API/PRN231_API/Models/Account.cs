using System;
using System.Collections.Generic;

namespace PRN231_API.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string ActiveCode { get; set; }
        public bool IsActive { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
