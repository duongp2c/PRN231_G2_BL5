using System;
using System.Collections.Generic;

namespace PRN231_API.Models
{
    public partial class StudentSubject
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public bool? IsComplete { get; set; }

        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
