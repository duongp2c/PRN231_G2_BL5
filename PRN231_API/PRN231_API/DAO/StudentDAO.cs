
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;

namespace PRN231_API.DAO
{
    public class StudentDAO
    {
        private readonly IStudentRepository _studentRepository;
        

        public StudentDAO(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
           
        }


        public async Task<string> RegisterSubjectAsync(int subjectId, int accountId)
        {
            // Lấy AccountId từ session


            if (accountId == null)
            {
                return "Account not found in session.";
            }

            // Lấy studentId dựa trên AccountId
            var studentId = await _studentRepository.GetStudentIdByAccountIdAsync(accountId);

            if (studentId == null)
            {
                return "Sinh viên không được tìm thấy cho tài khoản này";
            }

            // Fetch the student and their subjects
            var student = await _studentRepository.GetStudentByIdAsync(studentId.Value);
            if (student == null)
                return "Không tìm thấy sinh viên";

            var studentSubjects = await _studentRepository.GetStudentSubjectsAsync(studentId.Value);

            // Kiểm tra nếu sinh viên đã đăng ký 5 môn học
            if (studentSubjects.Count >= 5)
            {
                // Kiểm tra xem có môn học nào đã hoàn thành không
                bool hasCompletedSubject = studentSubjects.Any(ss => ss.IsComplete == true);
                if (!hasCompletedSubject)
                    return "Bạn chỉ có thể đăng ký môn học mới nếu bạn đã hoàn thành một trong những môn học hiện tại.";
            }

            // Kiểm tra xem sinh viên đã đăng ký môn học này chưa
            var existingSubject = studentSubjects.Any(ss => ss.SubjectId == subjectId);
            if (existingSubject)
                return "Bạn đã đăng ký cho môn học này.";

            // Đăng ký môn học mới
            var studentSubject = new StudentSubject
            {
                StudentId = studentId.Value,
                SubjectId = subjectId,
                IsComplete = false // Mặc định là chưa hoàn thành khi đăng ký
            };

            await _studentRepository.AddStudentSubjectAsync(studentSubject);
            return "Chúc mừng bạn đã đăng kí thành công";
        }
        // Phương thức trả về danh sách sinh viên bao gồm các thông tin chi tiết và môn học
        public async Task<List<StudentDTO>> GetAllStudentsWithDetailsAndSubjectsAsync()
        {
            // Lấy tất cả các sinh viên từ repository
            var students = await _studentRepository.GetAllStudentAsync();

            // Trả về danh sách sinh viên bao gồm chi tiết và môn học
            return students;
        }
        public async Task<string> DeleteStudentAsync(int studentId)
        {
            return await _studentRepository.DeleteStudentAsync(studentId);
        }

        public async Task<bool> EditActiveStudentAsync(int studentId, bool isActive)
        {
            return await _studentRepository.EditActiveStudentAsync(studentId, isActive);
        }
        public async Task<List<StudentDTO>> SearchStudentsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await GetAllStudentsWithDetailsAndSubjectsAsync();
            }

            // Perform search if name is provided
            var students = await _studentRepository.SearchStudentsByNameAsync(name);
            return students;
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
        public async Task<List<Subject1DTO>> GetStudentSubjectAsync(int accountId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(accountId);
            List<StudentSubject> ss = await _studentRepository.GetStudentSubjectsAsync(student.StudentId);
            List<Subject1DTO> subjects = new List<Subject1DTO>();
            foreach (var subject in ss) 
            {
                Subject1DTO s = new Subject1DTO
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
