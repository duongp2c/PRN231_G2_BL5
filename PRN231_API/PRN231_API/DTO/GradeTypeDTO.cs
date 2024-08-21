using System.ComponentModel.DataAnnotations;

namespace PRN231_API.DTO
{
    public class GradeTypeDTO
    {
        public int GradeTypeId { get; set; }
        public string GradeTypeName { get; set; }
        public decimal GradeTypeWeight { get; set; }
        public List<SubjectDTO> Subjects { get; set; }
    }
    public class CreateGradeTypeDTO
    {
        public int GradeTypeID { get; set; }
        [Required(ErrorMessage = "GradeTypeName is required")]
        public string GradeTypeName { get; set; }
        [Required(ErrorMessage = "GradeTypeWeight is required")]
        public decimal GradeTypeWeight { get; set; }
    }
}
