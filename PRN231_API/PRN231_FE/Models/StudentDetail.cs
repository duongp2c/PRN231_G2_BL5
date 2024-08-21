using System;
using System.Collections.Generic;

namespace PRN231_FE.Models
{
    public partial class StudentDetail
    {
        public int StudentDetailsId { get; set; }
        public int? StudentId { get; set; }
        public string? Address { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }

        public virtual Student? Student { get; set; }
    }
}
