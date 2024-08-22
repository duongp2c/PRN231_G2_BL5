
using PRN231_API.Models;
using System.ComponentModel.DataAnnotations;


namespace PRN231_API.DTO
{
    public class Subject1DTO
    {
        public string Name { get; set; }
        public int SubjectId { get; set; }
    }
    
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public TeacherDTO Teachers { get; set; }
        public List<GradeTypeDTO> Grades { get; set; }
        public int? TeacherId { get; set; }
        public string TeacherName { get; set; }
    }

    public class CreateSubjectDTO
    {
        [Required(ErrorMessage = "SubjectName is required")]
        public string SubjectName { get; set; }
        public int TeacherId { get; set; }
    }

    public class SubjectIsAndNameDTO
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

    }


}

