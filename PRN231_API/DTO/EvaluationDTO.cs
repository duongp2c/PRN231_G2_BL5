namespace PRN231_API.DTO
{
    public class EvaluationDTO
    {
        public int EvaluationId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }  // Added
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }   // Added
        public int GradeTypeId { get; set; }
        public string GradeTypeName { get; set; } // Added
        public decimal? Grade { get; set; }
        public string? AdditionExplanation { get; set; }
    }
}
