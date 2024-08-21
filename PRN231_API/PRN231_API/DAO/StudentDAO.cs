using PRN231_API.Models;
using PRN231_API.Repository;
using PRN231_API.DTO;

namespace PRN231_API.DAO
{
    public class StudentDAO
    {
        private readonly IStudentRepository _studentRepository;


        public StudentDAO(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;

        }
        public async Task<ProfileDTO> GetStudentDetailAsync(int accountId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(accountId);
            var studentDetail = await _studentRepository.GetStudentDetailByIdAsync(student.StudentId);
            var profile = new ProfileDTO { 
                Name = student.Name,
                Age = student.Age,
                Address = studentDetail.Address,
                AdditionalInfo = studentDetail.AdditionalInformation,
                Phone = studentDetail.Phone,
                Image = studentDetail.Image
            };
            return profile;
        }
        public async Task<List<SubjectDTO>> GetStudentSubjectAsync(int accountId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(accountId);
            List<StudentSubject> ss = await _studentRepository.GetStudentSubjectsAsync(student.StudentId);
            List<SubjectDTO> subjects = new List<SubjectDTO>();
            foreach (var subject in ss) 
            {
                SubjectDTO s = new SubjectDTO
                {
                    Name = subject.Subject.SubjectName,
                    SubjectId = subject.Subject.SubjectId
                };
                subjects.Add(s);
            }
            return subjects;
        }
        public async Task<string> UpdateStudentDetailAsync(ProfileDTO profile , int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);//sua sau khi co session
            if (student == null)
                return "Student not found.";
            var studentDetail = await _studentRepository.GetStudentDetailByIdAsync(id);//sua sau khi co session
            if (studentDetail == null)
                return "StudentDetail not found.";
            student.Name = profile.Name;
            student.Age = profile.Age;
            studentDetail.Address = profile.Address;
            studentDetail.AdditionalInformation = profile.AdditionalInfo;
            studentDetail.Phone = profile.Phone;  
            studentDetail.Image = profile.Image;
            await _studentRepository.UpdateStudentAsync(student);
            await _studentRepository.UpdateStudentDetailAsync(studentDetail);

            return "Update success";
        }


    }
}
