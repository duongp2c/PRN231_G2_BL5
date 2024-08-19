using System;
using System.Collections.Generic;

namespace PRN231_API.Models
{
    public partial class GradeType
    {
        public GradeType()
        {
            Evaluations = new HashSet<Evaluation>();
        }

        public int GradeTypeId { get; set; }
        public string GradeTypeName { get; set; } = null!;
        public decimal? GradeTypeWeight { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
