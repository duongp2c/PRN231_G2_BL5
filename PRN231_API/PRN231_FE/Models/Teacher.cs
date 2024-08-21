using System;
using System.Collections.Generic;

namespace PRN231_FE.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Subjects = new HashSet<Subject>();
        }

        public int TeacherId { get; set; }
        public int? AccountId { get; set; }
        public string? TeacherName { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
