using System.ComponentModel.DataAnnotations;

namespace PRN231_API.DTO
{
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public int AccountId { get; set; }
        public string TeacherName { get; set; }
        public string Email { get; set; }
        public List<SubjectDTO> Subjects { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreateTeacherDTO
    {
        public int TeacherId { get; set; }
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Teacher name is required.")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid email format. Email must be from Gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string Type = "Teacher";
        public bool IsActive { get; set; }
     
    }
    public class UpdateTeacherDTO
    {
        public bool IsActive { get; set; }
        public List<int> SubjectIds { get; set; } // Added for updating subjects
    }

}