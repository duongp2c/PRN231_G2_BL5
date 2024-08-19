using System;
using System.Collections.Generic;

namespace PRN231_API.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Evaluations = new HashSet<Evaluation>();
            StudentSubjects = new HashSet<StudentSubject>();
        }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int? TeacherId { get; set; }

        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
