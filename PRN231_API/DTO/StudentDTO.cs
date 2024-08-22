namespace PRN231_API.DTO
{
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsRegularStudent { get; set; }

        // Additional properties
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }
    }
}
