using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentByIdAsync(int studentId);
        Task<List<StudentSubject>> GetStudentSubjectsAsync(int studentId);
        Task AddStudentSubjectAsync(StudentSubject studentSubject);
        Task<int?> GetStudentIdByAccountIdAsync(int accountId);
        //Task<Student?> GetStudentImagesByAccountID(int accountId);
    }
}
