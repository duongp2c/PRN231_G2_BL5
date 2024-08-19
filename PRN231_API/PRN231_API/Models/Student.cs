using System;
using System.Collections.Generic;

namespace PRN231_API.Models
{
    public partial class Student
    {
        public Student()
        {
            Evaluations = new HashSet<Evaluation>();
            StudentSubjects = new HashSet<StudentSubject>();
        }

        public int StudentId { get; set; }
        public int? AccountId { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public bool IsRegularStudent { get; set; }

        public virtual Account? Account { get; set; }
        public virtual StudentDetail? StudentDetail { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
