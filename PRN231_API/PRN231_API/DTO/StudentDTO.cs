using PRN231_API.Models;

namespace PRN231_API.DTO
{
    public class StudentDTO
    {
      
        public int StudentId { get; set; }
        public int AccountId { get; set; }    
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsRegularStudent { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public List<SubjectDTO> Subjects { get; set; }
        public List<StudentDetail> StudentDetails { get; set; }
    }

}
