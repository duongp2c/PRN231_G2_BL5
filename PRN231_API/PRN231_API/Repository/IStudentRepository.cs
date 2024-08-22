
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface IStudentRepository
    {
        Task<List<StudentDTO>> GetAllStudentAsync();
        Task<Student?> GetStudentByIdAsync(int studentId);
        Task<List<StudentSubject>> GetStudentSubjectsAsync(int studentId);
        Task AddStudentSubjectAsync(StudentSubject studentSubject);
        Task<bool> EditActiveStudentAsync(int studentId, bool isActive);
        Task<string> DeleteStudentAsync(int studentId);
        Task<List<StudentDTO>> SearchStudentsByNameAsync(string name);
        Task<Student?> GetStudentByAccountIdAsync(int studentId); // hàm này của quốc
        Task<StudentDetail?> GetStudentDetailByIdAsync(int studentId);
        Task<List<StudentSubject>> GetStudentSubjectsByStudentIdAsync(int studentId);
        Task UpdateStudentAsync(Student student);
        Task UpdateStudentDetailAsync(StudentDetail student);
        //--------------------------------------------------------------------
        Task<int?> GetStudentIdByAccountIdAsync(int accountId);
    }
}
