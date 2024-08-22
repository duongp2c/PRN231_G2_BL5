using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentByAccountIdAsync(int studentId); // hàm này của quốc
        Task<StudentDetail?> GetStudentDetailByIdAsync(int studentId);
        Task<List<StudentSubject>> GetStudentSubjectsByStudentIdAsync(int studentId);
        Task UpdateStudentAsync(Student student);
        Task UpdateStudentDetailAsync(StudentDetail student);
        //--------------------------------------------------------------------
        Task<Student?> GetStudentByIdAsync(int studentId);
        Task<List<StudentSubject>> GetStudentSubjectsAsync(int studentId);
        Task AddStudentSubjectAsync(StudentSubject studentSubject);
        Task<int?> GetStudentIdByAccountIdAsync(int accountId);
    }
}
