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




    }
}
