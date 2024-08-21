using PRN231_API.Models;

namespace PRN231_API.DTO
{
    public class GradeTypeSubjectDTO
    {
        public int GreadeTypeId { get; set; }
        public int SubjectId { get; set; }
        public  GradeType GradeType { get; set; } = null!;
        public  Subject Subject { get; set; } = null!;
    }
}
