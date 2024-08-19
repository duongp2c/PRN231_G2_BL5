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
        public async Task<ProfileDTO> GetStudentDetailAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            var studentDetail = await _studentRepository.GetStudentDetailByIdAsync(studentId);
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
        public async Task<string> UpdateStudentDetailAsync(ProfileDTO profile)
        {
            var student = await _studentRepository.GetStudentByIdAsync(0);//sua sau khi co session
            if (student == null)
                return "Student not found.";
            var studentDetail = await _studentRepository.GetStudentDetailByIdAsync(0);//sua sau khi co session
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
