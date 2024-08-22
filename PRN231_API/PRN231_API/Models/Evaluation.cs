using System;
using System.Collections.Generic;

namespace PRN231_API.Models
{
    public partial class Evaluation
    {
       public int EvaluationId { get; set; }
        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public int? GradeTypeId { get; set; }
        public decimal? Grade { get; set; }
        public string? AdditionExplanation { get; set; }

        public virtual GradeType? GradeType { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Subject? Subject { get; set; }
    }
}

