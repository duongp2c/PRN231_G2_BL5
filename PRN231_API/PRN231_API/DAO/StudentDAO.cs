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
        public async Task<string> RegisterSubjectAsync(int subjectId, HttpContext httpContext)
        {
            // Lấy AccountId từ session
            var accountId = httpContext.Session.GetInt32("AccountId");

            if (accountId == null)
            {
                return "Account not found in session.";
            }

            // Lấy studentId dựa trên AccountId
            var studentId = await _studentRepository.GetStudentIdByAccountIdAsync(accountId.Value);

            if (studentId == null)
            {
                return "Student not found for this account.";
            }

            // Fetch the student and their subjects
            var student = await _studentRepository.GetStudentByIdAsync(studentId.Value);
            if (student == null)
                return "Student not found.";

            var studentSubjects = await _studentRepository.GetStudentSubjectsAsync(studentId.Value);

            // Kiểm tra nếu sinh viên đã đăng ký 5 môn học
            if (studentSubjects.Count >= 5)
            {
                // Kiểm tra xem có môn học nào đã hoàn thành không
                bool hasCompletedSubject = studentSubjects.Any(ss => ss.IsComplete == true);
                if (!hasCompletedSubject)
                    return "You can only register for a new subject if you have completed one of the existing subjects.";
            }

            // Kiểm tra xem sinh viên đã đăng ký môn học này chưa
            var existingSubject = studentSubjects.Any(ss => ss.SubjectId == subjectId);
            if (existingSubject)
                return "You are already registered for this subject.";

            // Đăng ký môn học mới
            var studentSubject = new StudentSubject
            {
                StudentId = studentId.Value,
                SubjectId = subjectId,
                IsComplete = false // Mặc định là chưa hoàn thành khi đăng ký
            };

            await _studentRepository.AddStudentSubjectAsync(studentSubject);
            return "Subject registered successfully.";
        }




    }
}
