using PRN231_API.Models;
using System.ComponentModel.DataAnnotations;

namespace PRN231_API.DTO
{
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        
        public List<TeacherDTO> Teachers { get; set; }
        public List<GradeTypeDTO> Grades { get; set; }
    }
    public class CreateSubjectDTO
    {
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "SubjectName is required")]
        public string SubjectName { get; set; }
        public List<int> TeacherIds { get; set; }
    }
}

