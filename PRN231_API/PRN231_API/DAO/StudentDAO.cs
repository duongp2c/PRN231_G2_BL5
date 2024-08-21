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
        public async Task<string> RegisterSubjectAsync(int studentId, int subjectId)
        {
            // Fetch the student and their subjects
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                return "Student not found.";

            var studentSubjects = await _studentRepository.GetStudentSubjectsAsync(studentId);

            // Check if the student already has 5 subjects
            if (studentSubjects.Count >= 5)
            {
                // Check if any of the existing subjects is complete
                bool hasCompletedSubject = studentSubjects.Any(ss => ss.IsComplete == true);
                if (!hasCompletedSubject)
                    return "You can only register for a new subject if you have completed one of the existing subjects.";
            }

            // Check if the subject is already registered
            var existingSubject = studentSubjects.Any(ss => ss.SubjectId == subjectId);
            if (existingSubject)
                return "You are already registered for this subject.";

            // Register the new subject
            var studentSubject = new StudentSubject
            {
                StudentId = studentId,
                SubjectId = subjectId,
                IsComplete = false // Default to false when registering 
            };

            await _studentRepository.AddStudentSubjectAsync(studentSubject);
            return "Subject registered successfully.";
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


    }
}
